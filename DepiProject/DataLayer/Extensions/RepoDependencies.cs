using DataLayer.Repository;
using DataLayer.Repository.IRepository;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer.Extensions;

public static class RepoDependencies
{
    public static IServiceCollection AddRepoDependencies(this IServiceCollection services)
    {
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IProductImageRepository, ProductImageRepository>();


        return services;
    }
}