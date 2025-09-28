namespace GoogleLogin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddCookie().AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                //options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
                //options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
                options.ClientId = Environment.GetEnvironmentVariable("ClientId");
                options.ClientSecret = Environment.GetEnvironmentVariable("ClientSecret");
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
