using AspNetCoreRazor.Security.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace AspNetCoreRazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Pour les test de Jwt
            builder.Services.AddHttpClient();
            builder.Services.AddLogging();
            builder.Services.AddSingleton<JwtTokenTest>(new JwtTokenTest());

            // Ajouter OpenID (qui contient Oauth 2.0)
            var clientId = "e8b7d6f4-85c6-4a33-bb7d-8b715089b7a3";
            var clientSecret = "pV~5D8s7Z3W2j5!gH7kL3^q#1P!TzR4A";

            builder.Services.AddAuthentication(o =>
                {
                    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                }
            )
            .AddCookie(options =>
                {
                    //options.Cookie.HttpOnly = true;
                    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
         
                }
            )
            .AddOpenIdConnect(options =>
            {
                options.Authority = "https://localhost:7180"; // L'URL de votre fournisseur d'OpenID Connect
                options.ClientId = clientId; //  Configuration["Authentication:OIDC:ClientId"];
                options.ClientSecret = clientSecret; // Configuration["Authentication:OIDC:ClientSecret"];
                options.ResponseType = "code"; // Utilisation du flux de code d'autorisation

                options.SaveTokens = true; // Enregistrer les jetons pour une utilisation ultérieure

                options.Scope.Add("profile");
                options.Scope.Add("email");

                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            
                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = context =>
                    {
                        // Traitement supplémentaire après validation du jeton
                        return Task.CompletedTask;
                    }
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
