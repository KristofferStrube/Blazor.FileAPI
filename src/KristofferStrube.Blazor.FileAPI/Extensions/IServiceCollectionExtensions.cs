using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddURLService(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<IURLService, URLService>();
    }

    public static IServiceCollection AddURLServiceInProcess(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<IURLServiceInProcess>(sp => new URLServiceInProcess((IJSInProcessRuntime)sp.GetRequiredService<IJSRuntime>()));
    }
}
