namespace AthamePlugin.Tidal.InternalApi.Decryption
{
    public class FileKey
    {
        public byte[] DecryptionKey { get; set; }
        public byte[] Nonce { get; set; }
    }
}