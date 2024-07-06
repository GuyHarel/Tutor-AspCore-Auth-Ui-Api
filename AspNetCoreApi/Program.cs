
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

            // Authentification JWT Bearer
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = "AspNetCoreApi.csproj",
                            ValidAudience = "AspNetCoreRazor.csproj",
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey128Bits)),
                            NameClaimType = ClaimTypes.Name,
                            RoleClaimType = ClaimTypes.Role,
                            // Custom claim validation for "sub"
                            ValidateTokenReplay = true,
                            ValidateActor = true,
                            RequireExpirationTime = true,
                            RequireSignedTokens = true
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnTokenValidated = context =>
                            {
                                var userClaims = context.Principal.Claims;
                                var subjectClaim = userClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name);

                                if (subjectClaim == null || subjectClaim.Value != "bibi2")
                                {
                                    context.Fail("Unauthorized");
                                }

                                return Task.CompletedTask;
                            }
                        };

                    });

            builder.Services.AddLogging();
            //builder.Services.AddHttpLogging(logging =>
            //{
            //    logging.LoggingFields = HttpLoggingFields.All;
            //    logging.RequestHeaders.Add("Referer");
            //    logging.RequestHeaders.Add("Origin");
            //    logging.RequestHeaders.Add("X-Forwarded-For");
            //    logging.RequestHeaders.Add("X-Forwarded-Host");
            //    logging.RequestHeaders.Add("X-Forwarded-Proto");
            //    logging.RequestHeaders.Add("Upgrade-Insecure-Requests");
            //    logging.RequestHeaders.Add("Sec-Fetch-Site");
            //    logging.RequestHeaders.Add("Sec-Fetch-Mode");
            //    logging.RequestHeaders.Add("Sec-Fetch-Dest");
            //    logging.RequestHeaders.Add("Access-Control-Request-Method");
            //    logging.RequestHeaders.Add("Access-Control-Request-Headers");
            //    logging.ResponseHeaders.Add("Access-Control-Allow-Origin");
            //    logging.ResponseHeaders.Add("Access-Control-Allow-Methods");
            //    logging.ResponseHeaders.Add("Access-Control-Request-Headers");
            //    logging.ResponseHeaders.Add("Access-Control-Allow-Credentials");
            //    logging.ResponseHeaders.Add("Access-Control-Max-Age");
            //    logging.MediaTypeOptions.AddText("application/javascript");
            //    logging.RequestBodyLogLimit = 4096;
            //    logging.ResponseBodyLogLimit = 4096;
            //});

            //builder.Services.AddAuthentication()
            //    .AddJwtBearer(o =>
            //    {
            //        o.Authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";
            //        o.Audience = clientId;
            //        o.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = "gh-AspNetCoreApi.com",
            //            ValidAudience = "gh-AspNetCoreApi.com",
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey128Bits))
            //        };
            //    });

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
