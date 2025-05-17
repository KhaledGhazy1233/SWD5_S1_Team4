using BusinessLayer.Services;
using BusinessLayer.Services.Implementation;
using BusinessLayer.Services.Interface;
using DataLayer.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.Extensions;

public static class FilesDependencies
{
    public static IServiceCollection AddFilesDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudinarySettings>(
        configuration.GetSection("CloudinarySettings"));
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IPhotoUploadService, PhotoUploadService>();

        return services;
    }
}