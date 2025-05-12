using BusinessLayer.Services.Implementation;
using BusinessLayer.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.Extensions;

public static class InterfacesDependencies
{
    public static IServiceCollection AddInterfacesDependencies(this IServiceCollection services)
    {
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<ICategoryService, CategoryService>();

        return services;
    }
}