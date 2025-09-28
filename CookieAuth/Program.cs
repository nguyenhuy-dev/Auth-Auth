using CookieAuthRepo;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CookieAuth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.Configure<JsonFileUserRepositoryOptions>(builder.Configuration.GetSection("JsonDatabaseOptions"));
            builder.Services.AddScoped<IUserRepository, JsonFileUserRepository>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, (options) =>
            {
                options.LoginPath = "/CookieAuth/LogOn";
                options.AccessDeniedPath = "/CookieAuth/LogOn";
                options.LogoutPath = "/CookieAuth/LogOff";
                options.Cookie.Name = "CookieAuth";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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

            app.Run();
        }
    }
}
