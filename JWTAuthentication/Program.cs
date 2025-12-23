using Microsoft.OpenApi.Models;
using System.Globalization;

namespace JWTAuthentication
{
    static class Program
    {
        private static readonly IFormatProvider? enUs = new CultureInfo("en-US");

        static void Main(string[] args)
        {
            IdentityModelEventSource.ShowPII = true;

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    //chứa thông tin cách mà ta sẽ validate 1 cái token
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "firehorse.org",
                        ValidAudience = "firehorse.org",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes("nguyenquochuynguyenquochuynguyenquochuy"))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("Event here");
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("Policy1", policy => policy.RequireRole("Student", "Teacher")
                                                        .RequireClaim("client-id", "client1")
                                                        .RequireClaim("client-id", "client2"))
                .AddPolicy("Policy2", policy => policy.RequireRole("Student")
                                                        .RequireRole("Teacher")
                                                        .RequireUserName("Nguyen Mai"))
                .AddPolicy("Policy3", policy => policy.RequireAssertion(context => context.User.Identity?.Name?.StartsWith("Nguyen", StringComparison.OrdinalIgnoreCase) ?? false))
                .AddPolicy("Policy4", policy => policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth")
                                                        .RequireAssertion(context =>
                                                        {
                                                            string dateString = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth")!.Value;
                                                            if (String.IsNullOrEmpty(dateString)) return false;

                                                            bool isDate = DateOnly.TryParseExact(dateString, "yyyy-MM-dd", enUs, DateTimeStyles.None, out DateOnly date) && (date <= DateOnly.FromDateTime(DateTime.Now).AddYears(-18));
                                                            if (!isDate) return false;

                                                            return true;
                                                        }));

            // Add Swagger UI
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                    c.RoutePrefix = string.Empty; // Swagger UI in root "/"
                });
            }

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
