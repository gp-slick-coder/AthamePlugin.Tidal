using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Downloader;
using AthamePlugin.Tidal.InternalApi.Decryption;

namespace AthamePlugin.Tidal
{
    public class TidalEncryptedDownloader : IDownloader
    {
        public async Task DownloadAsyncTask(TrackFile track, string destination)
        {
            var tidalTrack = (TidalTrackFile) track;
            var request = WebRequest.CreateHttp(tidalTrack.DownloadUri);
            request.Method = "GET";
            var response = (HttpWebResponse) await request.GetResponseAsync();
            using (
                var cryptoStream = TidalDecryptor.CreateDecryptionStream(tidalTrack.FileKey,
                    response.GetResponseStream()))
            {
                using (var destFile = File.OpenWrite(destination))
                {
                    var eventArgs = new DownloadEventArgs {State = DownloadState.Downloading};
                    int bytesRead, bytesReadTotal = 0;
                    var buffer = new byte[TidalDecryptor.BlockSize];
                    while ((bytesRead = await cryptoStream.ReadAsync(buffer, 0, TidalDecryptor.BlockSize)) > 0)
                    {
                        bytesReadTotal += bytesRead;
                        await destFile.WriteAsync(buffer, 0, bytesRead);
                        eventArgs.PercentCompleted = (decimal)bytesReadTotal / response.ContentLength;
                        Progress?.Invoke(this, eventArgs);
                    }
                }
            }
            Done?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<DownloadEventArgs> Progress;
        public event EventHandler Done;
    }
}



