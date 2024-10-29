using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.FileAPI;

/// <summary>
/// Contains extension methods for adding services to a <see cref="IServiceCollection"/>.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds an <see cref="IURLService"/> to the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    public static IServiceCollection AddURLService(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<IURLService, URLService>();
    }

    /// <summary>
    /// Adds an <see cref="IURLServiceInProcess"/> to the service collection.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    public static IServiceCollection AddURLServiceInProcess(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<IURLServiceInProcess>(sp => new URLServiceInProcess((IJSInProcessRuntime)sp.GetRequiredService<IJSRuntime>()));
    }
}
