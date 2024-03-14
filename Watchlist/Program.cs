using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
            /*** Original Automaticaly generated code by Visual Studio 2022
                builder.Services.AddDefaultIdentity<IdentityUser>
                    (options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();
                builder.Services.AddControllersWithViews();
            ***/

            //OCR objective: update code for managing users identity with the
            //extended Utilisateur class (extended from Microsoft.AspNetCore.Identity.IdentityUser).
            /*
             * ==> Code below from OCR not working and had to be fixed :
             * Nb. that code was supposed to be add in Startup.cs (and there is no more startup.cs in VS2022, that code is in program.cs
           
                services.AddIdentity<Utilisateur, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = false;
                })
                   .AddDefaultUI(UIFramework.Bootstrap4)
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddDefaultTokenProviders();
            */
            // Below the code from OCR updated by SMoureu (V1) :
            /*
             * First version of the code :
             * it appears that code is not enough improved for making the 
             * application correctly worked correctly, even if it raises no exception anymore
            
            builder.Services.AddIdentity<Utilisateur, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
             //SMO: non reconnu (UIFramework.Bootstrap4) dans le contexte 
             //.AddDefaultUI(UIFramework.Bootstrap4) 
             // contournement possible utiliser Bootstrap5
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();
            */
            //New updated code (V2) by SMoureu due to bug003 on App.UseAuthorization
            /* 
             * Updated V2 code compile but this exception raises on app execution ==>
             *      InvalidOperationException: Unable to find the required services. Please add all the required services by calling 'IServiceCollection.AddRazorPages' inside the call to 'ConfigureServices(...)' in the application startup code.

                 builder.Services.AddIdentity<Utilisateur, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = false;
                })
                 //SMO: non reconnu (UIFramework.Bootstrap4) dans le contexte 
                 //.AddDefaultUI(UIFramework.Bootstrap4) 
                 // contournement possible utiliser Bootstrap5
                 .AddEntityFrameworkStores<ApplicationDbContext>()
                 .AddDefaultTokenProviders();
                builder.Services.AddControllersWithViews();
            */

            //V3 code from SMoureu below
            //  ===> we add missing original instructions from automatically
            //  generated code in VS2022 to fixed OCR code
            builder.Services.AddIdentity<Utilisateur, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
             //SMO: non reconnu (UIFramework.Bootstrap4) dans le contexte 
             //.AddDefaultUI(UIFramework.Bootstrap4) 
             // contournement possible utiliser Bootstrap5
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();
            builder.Services.AddControllersWithViews();
            var app = builder.Build();
            

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
            //BUG003 :
            //System.InvalidOperationException : 'Unable to find the required services.
            //Please add all the required services by calling 'IServiceCollection.AddAuthorization'
            //in the application startup code.'
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}