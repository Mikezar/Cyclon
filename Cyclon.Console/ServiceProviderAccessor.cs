namespace Cyclon.Console;

public static class ServiceProviderAccessor
{
    private static IServiceProvider? _serviceProvider;

    public static IServiceProvider ServiceProvider
    {
        get
        {
            if (_serviceProvider == null) 
            {
                throw new InvalidOperationException("ServiceProviderAccessor is not created");
            }

            return _serviceProvider;
        }
    }

    public static void Create(IServiceProvider serviceProvider)
    {
        _serviceProvider ??= serviceProvider;
    }
}

