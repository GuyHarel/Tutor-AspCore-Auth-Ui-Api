using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using System.Net.Http;

namespace AspNetCoreRazor.Pages
{
    public class AppelModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AppelModel( IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(string token)
        {
            // Obtenir le token JWT du serveur AspNetCoreApi

            var httpClient = httpClientFactory.CreateClient();
            var reponse = httpClient.Send(CreerRequeteMessage(token));

            return new JsonResult(new { statut = reponse.StatusCode, content = reponse.Content.ReadAsStringAsync().Result });
        }

        private HttpRequestMessage CreerRequeteMessage(string token)
        {
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://localhost:7180/WeatherForecast")
            {
                Headers =
                {
                    { HeaderNames.Authorization, $"Bearer {token}" }
                }
            };

            return request;
        }
    }
}
