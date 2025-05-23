using BusinessLayer.Extensions;
using BusinessLayer.Services;
using DataLayer.Extensions;
using DataLayer.Repository;
using DataLayer.Repository.IRepository;
using Stripe;
//using DataLayer.Seed;

namespace DepiProject;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddHttpClient();
        // Add Identity services
        builder.Services.AddIdentityServices(builder.Configuration)
                        .AddFilesDependencies(builder.Configuration)
                        .AddInterfacesDependencies()
                        .AddRepoDependencies();
        // Add repositories and unit of work
        builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Add services
        builder.Services.AddScoped<IEmailService, EmailService>();

        // Add RoleInitializer as a hosted service
        builder.Services.AddHostedService<RoleInitializer>();

        builder.Services.AddHttpClient(); var app = builder.Build();

        // Configure Stripe API key
        StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization(); app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Seed the database with initial data
        app.Services.CreateScope().ServiceProvider.GetRequiredService<ILogger<Program>>()
            .LogInformation("Starting database seeding");
        app.SeedDatabase();

        app.Run();
    }
}
