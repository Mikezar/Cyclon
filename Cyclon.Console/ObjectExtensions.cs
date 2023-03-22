namespace Cyclon.Console;

public static class ObjectExtensions
{
    public static string OrThrow(this string? parameter)
    {
        if (parameter is null)
        {
            throw new NullReferenceException(nameof(parameter));
        }

        return parameter;
    }
}