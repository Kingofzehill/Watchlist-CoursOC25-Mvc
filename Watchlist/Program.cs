using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Watchlist.Data;

namespace Watchlist
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //BUG001
            //Objective: update code for managing users identity with the
            //extended Utilisateur class (extended from Microsoft.AspNetCore.Identity.IdentityUser).
            /***
                builder.Services.AddDefaultIdentity<IdentityUser>
                    (options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();
                builder.Services.AddControllersWithViews();
            ***/
            //Code from OC not working and should be fixed as below :
            // add builder. to Services.AddIdentity
            // comment .AddDefaultUI(UIFramework.Bootstrap4) : not included in Visual Studio 2022
            builder.Services.AddIdentity<Utilisateur, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
             //SMO: non reconnu (UIFramework.Bootstrap4) dans le contexte 
             //.AddDefaultUI(UIFramework.Bootstrap4) 
             // contournement possible utiliser Bootstrap5
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();
            var app = builder.Build();
            // end SMO001.

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}