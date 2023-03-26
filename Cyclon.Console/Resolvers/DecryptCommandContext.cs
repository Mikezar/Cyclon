namespace Cyclon.Console.Resolvers;

internal sealed class DecryptCommandContext : IResolutionContext
{
    public DecryptCommandContext(string filePath, string passphrase)
    {
        FilePath = filePath;
        Passphrase = passphrase;
    }

    public string FilePath { get; }
    public string Passphrase { get; }
}
