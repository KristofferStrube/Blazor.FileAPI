using KristofferStrube.Blazor.FileAPI.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddURLService(this IServiceCollection serviceCollection, Action<FileApiOptions>? configure)
    {
        serviceCollection.ConfigureFaOptions(configure);
        return serviceCollection.AddScoped<IURLService, URLService>();
    }

    public static IServiceCollection AddURLServiceInProcess(this IServiceCollection serviceCollection, Action<FileApiOptions>? configure)
    {
        serviceCollection.ConfigureFaOptions(configure);
        return serviceCollection.AddScoped<IURLServiceInProcess>(sp => new URLServiceInProcess((IJSInProcessRuntime)sp.GetRequiredService<IJSRuntime>()));
    }

    private static void ConfigureFaOptions(this IServiceCollection services, Action<FileApiOptions>? configure)
    {
        if (configure is null) return;

        services.Configure(configure);
        configure(FileApiOptions.DefaultInstance);
    }
}