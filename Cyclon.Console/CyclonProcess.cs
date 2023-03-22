namespace Cyclon.Console;

internal static class CyclonProcess
{
    private static readonly string AppGuid = "6a659f46-8365-4381-bf90-c27ae92eee19";

    public static void Start(
        Func<CancellationToken, IServiceProvider> dependencyRegistryFunc, 
        Func<IServiceProvider, Task> application, 
        CancellationToken cancellationToken)
    {
        using (var mutex = new Mutex(false, AppGuid))
        {
            try
            {
                try
                {
                    if (!mutex.WaitOne(0, false))
                    {
                        throw new AbandonedMutexException("Another instance is already running.");
                    }
                }
                catch (AbandonedMutexException)
                {
                }

                Task.Run(async () => {
                    var provider = dependencyRegistryFunc(cancellationToken);
                    await application(provider);
                }, cancellationToken).Wait(cancellationToken);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}