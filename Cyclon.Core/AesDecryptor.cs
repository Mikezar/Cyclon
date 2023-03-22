using System.Security.Cryptography;

namespace Cyclon.Core;

public sealed class AesDecryptor : IDecryptor
{
    private readonly IWriter _writer;

    public AesDecryptor(IWriter writer)
    {
        _writer = writer;
    }

    public async Task Decrypt(Stream readStream, EncryptionOptions encryptionOptions, CancellationToken cancellationToken)
    {
        using var aes = Aes.Create();
        using var transform = aes.CreateDecryptor(encryptionOptions.Key, encryptionOptions.IV);
        using var writeStream = _writer.OpenStream();
        int count = 0;
        int offset = 0;
        int blockSizeBytes = aes.BlockSize / 8;
        byte[] data = new byte[blockSizeBytes];

        readStream.Seek(0, SeekOrigin.Begin);

        using var outStreamDecrypted = new CryptoStream(writeStream, transform, CryptoStreamMode.Write);
        do
        {
            count = await readStream.ReadAsync(data.AsMemory(0, blockSizeBytes), cancellationToken);
            offset += count;
            await outStreamDecrypted.WriteAsync(data.AsMemory(0, count), cancellationToken);
        } while (count > 0);

        await outStreamDecrypted.FlushFinalBlockAsync(cancellationToken);
    }
}
