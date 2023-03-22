namespace Cyclon.Core;

public static class FileUtilities
{
    public static string GetEncryptedFileName(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        var newfileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        return $"{newfileName}{extension}.enc";
    }

    public static string GetDecryptedFileName(string fileName)
    {
        var decryptedFielName = Path.GetFileNameWithoutExtension(fileName);
        return decryptedFielName;
    }

    public static IList<string> GetAllFilePaths(string filePath)
    {
        var filePaths = new List<string>();

        if (Directory.Exists(filePath))
        {
            var paths = Directory.GetFiles(filePath);
            filePaths.AddRange(paths);
        }
        else
        {
            filePaths.Add(filePath);
        }

        return filePaths;
    }
}
