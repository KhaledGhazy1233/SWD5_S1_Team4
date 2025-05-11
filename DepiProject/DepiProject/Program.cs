using BusinessLayer.Extensions;
using BusinessLayer.Services;
using DataLayer.Repository;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Identity;

namespace DepiProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            // Add Identity services
            builder.Services.AddIdentityServices(builder.Configuration);
            // Add repositories and unit of work
            //builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            // Add services
            builder.Services.AddScoped<IEmailService, EmailService>();
            
            // Add RoleInitializer as a hosted service
            builder.Services.AddHostedService<RoleInitializer>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
