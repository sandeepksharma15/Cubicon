using Microsoft.Extensions.DependencyInjection;

namespace Cubicon;

internal class AppServices
{
    internal static IServiceProvider RegisterServices()
    {
        var serviceCollection = new ServiceCollection();

        ConfigureServices(serviceCollection);

        return serviceCollection.BuildServiceProvider();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<IAppStateService, AppStateService>();
    }
}
