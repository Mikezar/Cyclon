namespace Cyclon.Core;

public interface IEncryptor
{
    Task Encrypt(Stream stream, EncryptionOptions encryptionOptions, CancellationToken cancellationToken);
}