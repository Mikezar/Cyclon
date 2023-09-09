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

    public static bool IsAFile(string path)
    {
        return !Directory.Exists(path);
    }
}
