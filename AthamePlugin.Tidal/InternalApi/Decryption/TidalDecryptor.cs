using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AthamePlugin.Tidal.InternalApi.Decryption
{
    public static class TidalDecryptor
    {
        private static readonly byte[] MasterKey =
            Convert.FromBase64String("UIlTTEMmmLfGowo/UC60x2H45W6MdGgTRfo/umg4754=");

        private static readonly RijndaelManaged MasterCipher = new RijndaelManaged();

        public const int BlockSize = 1024 * 1024;

        static TidalDecryptor()
        {
            MasterCipher.Mode = CipherMode.CBC;
            MasterCipher.Key = MasterKey;
        }

        public static FileKey ParseFileKey(byte[] encryptedFileKey)
        {
            using (var decryptor = MasterCipher.CreateDecryptor())
            {
                using (var encryptedFileKeyStream = new MemoryStream(encryptedFileKey))
                {
                    using (var cryptoStream = new CryptoStream(encryptedFileKeyStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var fileKeyStream = new MemoryStream())
                        {
                            cryptoStream.CopyTo(fileKeyStream);
                            var bytes = fileKeyStream.ToArray();
                            // [0:16] is junk
                            var important = bytes.Skip(16).ToArray();
                            // Key is [16:32]
                            var key = important.Take(16).ToArray();
                            // Nonce is [32:40], but we need to pad it out so 
                            // it can be used as the CTR counter
                            var nonce = new byte[16];
                            Array.Copy(important.Skip(16).ToArray(), nonce, 8);
                            return new FileKey
                            {
                                DecryptionKey = key,
                                Nonce = nonce
                            };
                        }
                    }
                }
            }
        }

        public static Stream CreateDecryptionStream(FileKey key, Stream inStream)
        {
            var fileCipher = new Aes128CounterMode(key.Nonce);
            var decryptor = fileCipher.CreateDecryptor(key.DecryptionKey, null);
            return new CryptoStream(inStream, decryptor, CryptoStreamMode.Read);
        } 
    }
}
