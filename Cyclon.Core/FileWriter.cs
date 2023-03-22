namespace Cyclon.Core;

public sealed class FileWriter : IWriter
{
    public string FilePath { get; }

    public FileWriter(string filePath)
    {
        FilePath = filePath;
    }

    public Stream OpenStream()
    {
        return new FileStream(FilePath, FileMode.Create);
    }
}
