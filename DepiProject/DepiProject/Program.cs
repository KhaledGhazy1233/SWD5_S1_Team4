using BusinessLayer.Extensions;
using BusinessLayer.Services;
using DataLayer.Context;
using DataLayer.Entities;
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
            // Repositories and unit of work
            //builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            builder.Services.AddScoped<IUnitOfWork>(serviceProvider => {
                var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                return new UnitOfWork(dbContext, userManager);
            });
            
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
            
            // Authentication/Authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
                
            // Add 404 handling middleware at the end of the pipeline
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Response.Redirect("/Home/NotFound");
                }
            });

            app.Run();
        }
    }
}
