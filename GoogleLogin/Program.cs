using System.Reflection;

namespace GoogleLogin
{
    static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Configuration.AddEnvironmentVariables();
            var configuration = builder.Configuration;

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddCookie().AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                var clientIdErrorMessage = "ClientId is missing";
                options.ClientId = Environment.GetEnvironmentVariable("ClientId") 
                                        ?? configuration.GetSection("GoogleKeys:ClientId").Value 
                                        ?? throw new InvalidDataException(clientIdErrorMessage);
                options.ClientSecret = Environment.GetEnvironmentVariable("ClientSecret") 
                                        ?? configuration.GetSection("GoogleKeys:ClientSecret").Value 
                                        ?? throw new InvalidDataException(clientIdErrorMessage);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
