using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Downloader;
using AthamePlugin.Tidal.InternalApi.Decryption;

namespace AthamePlugin.Tidal
{
    public class TidalEncryptedDownloader : IDownloader, IDisposable
    {
        private readonly WebClient mClient = new WebClient();

        public TidalEncryptedDownloader()
        {
            mClient.DownloadProgressChanged += OnDownloadProgressChanged;

        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs downloadProgressChangedEventArgs)
        {
            var percentage = (decimal)downloadProgressChangedEventArgs.BytesReceived /
                               downloadProgressChangedEventArgs.TotalBytesToReceive;
            var eventArgs = new DownloadEventArgs
            {
                State = DownloadState.Downloading,
                PercentCompleted = percentage
            };
            Progress?.Invoke(this, eventArgs);
        }

        public event EventHandler<DownloadEventArgs> Progress;
        public event EventHandler Done;

        public void Dispose()
        {
            mClient?.Dispose();
        }

        public async Task DownloadAsyncTask(TrackFile track, string destination)
        {
            var tidalTrack = (TidalTrackFile) track;
            // This is the way to do it as a stream. However this is also really slow.
//            var request = WebRequest.CreateHttp(tidalTrack.DownloadUri);
//            request.Method = "GET";
//            var response = (HttpWebResponse) await request.GetResponseAsync();
//            using (
//                var cryptoStream = TidalDecryptor.CreateDecryptionStream(tidalTrack.FileKey,
//                    response.GetResponseStream()))
//            {
//                using (var destFile = File.OpenWrite(destination))
//                {
//                    var eventArgs = new DownloadEventArgs {State = DownloadState.Downloading};
//                    int bytesRead, bytesReadTotal = 0;
//                    var buffer = new byte[TidalDecryptor.BlockSize];
//                    while ((bytesRead = await cryptoStream.ReadAsync(buffer, 0, TidalDecryptor.BlockSize)) > 0)
//                    {
//                        bytesReadTotal += bytesRead;
//                        await destFile.WriteAsync(buffer, 0, bytesRead);
//                        eventArgs.PercentCompleted = (decimal)bytesReadTotal / response.ContentLength;
//                        Progress?.Invoke(this, eventArgs);
//                    }
//                }
//            }
//            Done?.Invoke(this, EventArgs.Empty);
            var data = await mClient.DownloadDataTaskAsync(tidalTrack.DownloadUri);
            Progress?.Invoke(this, new DownloadEventArgs {PercentCompleted = 1m, State = DownloadState.PostProcess});
            using (var inputStream = new MemoryStream(data))
            {
                using (
                    var cryptoStream = TidalDecryptor.CreateDecryptionStream(tidalTrack.FileKey, inputStream))
                {
                    using (var destFile = File.OpenWrite(destination))
                    {
                        await cryptoStream.CopyToAsync(destFile, TidalDecryptor.BlockSize);
                    }
                }
            }
            Done?.Invoke(this, EventArgs.Empty);
        }
    }
}



