
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AspNetCoreApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // JwtAuthen
            var secretKey128Bits = "6b9d5e8f3a4b2c1d0e6f7a8b9c0d1e2f3b4c5d6e7f8a9b0c1d2e3f4g5h6i7j8k";

            // Japa341473
            // gh-util-api-test@guyharel13outlook.onmicrosoft.com
            var tenantId = "1a5bb257-5cdc-434d-b4cc-20e4218b3639";
            var clientId = "gh-util-api-test@guyharel13outlook.onmicrosoft.com";

            builder.Services.AddAuthentication()
                .AddJwtBearer(o =>
                {
                    o.Authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";
                    o.Audience = clientId;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "gh-AspNetCoreApi.com",
                        ValidAudience = "gh-AspNetCoreApi.com",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey128Bits))
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
