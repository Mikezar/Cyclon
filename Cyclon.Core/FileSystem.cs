namespace Cyclon.Core;

public class FileSystem
{
    public Folder RootFolder { get;}

    public FileSystem(string folderPath)
    {
        RootFolder = Init(folderPath);
    }

    private static Folder Init(string folderPath)
    {
        var folderName = new DirectoryInfo(folderPath).Name;
        var filePaths = Directory.GetFiles(folderPath).ToList();
        var directories = Directory.GetDirectories(folderPath).ToList();
        var innerFolders = new List<Folder>();

        foreach (var innerDirectory in directories)
        {
            var folder = Init(innerDirectory);
            innerFolders.Add(folder);
        }

        return new Folder(folderName, filePaths, innerFolders);
    }

    public record Folder(string Name, List<string> FilePaths, List<Folder> Folders);
}