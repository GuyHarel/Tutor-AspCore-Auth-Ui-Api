using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Net.Http.Headers;

namespace AspNetCoreRazor.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(string username, string password)
        {
            // Obtenir le token JWT du serveur AspNetCoreApi

            var httpClient = httpClientFactory.CreateClient();
            var reponse = httpClient.Send(CreerRequeteMessage(username, password));

            //var data = new FormUrlEncodedContent(new Dictionary<string, string>
            //{
            //    { "username", username },
            //    { "password", password }
            //});

            //var reponse = httpClient.Send(new HttpRequestMessage
            //{
            //    Method = HttpMethod.Post,
            //    RequestUri = new Uri("https://localhost:7180/api/jwt"),
            //    Headers =
            //    {
            //        { HeaderNames.Accept, "application/json" },
            //        { HeaderNames.UserAgent, "gh" }
            //    },
            //    Content = data
                
            //});

            return new JsonResult(new { statut = reponse.StatusCode, content = reponse.Content.ReadAsStringAsync().Result });
        }

        private HttpRequestMessage CreerRequeteMessage(string username, string password)
        {
            var data = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            });

            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://localhost:7180/api/jwt/obtenir-token")
            {
                Content = data
            };

            return request;
        }
    }
}
