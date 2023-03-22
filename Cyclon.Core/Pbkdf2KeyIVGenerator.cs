using System.Security.Cryptography;

namespace Cyclon.Core;

public sealed class Pbkdf2KeyIVGenerator : IKeyIVGenerator
{
    private const int _iterations = 1000;
    private const int _keyLength = 32;
    private const int _ivLength = 16;
    private readonly HashAlgorithmName _hashMethod = HashAlgorithmName.SHA384;

    public (byte[] key, byte[] iv) Generate(byte[] secret)
    {
        var emptySalt = Array.Empty<byte>();

        RandomNumberGenerator.Fill(emptySalt);

        var key = Rfc2898DeriveBytes.Pbkdf2(secret,
                                         emptySalt,
                                         _iterations,
                                         _hashMethod,
                                         _keyLength);

        var iv = Rfc2898DeriveBytes.Pbkdf2(secret,
                                         emptySalt,
                                         _iterations,
                                         _hashMethod,
                                         _ivLength);
        return (key, iv);
    }
}