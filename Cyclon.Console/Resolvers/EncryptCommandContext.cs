namespace Cyclon.Console.Resolvers;

internal sealed class EncryptCommandContext : IResolutionContext
{
    public EncryptCommandContext(string filePath, string passphrase)
    {
        FilePath = filePath;
        Passphrase = passphrase;
    }

    public string FilePath { get; }
    public string Passphrase { get; }
}
