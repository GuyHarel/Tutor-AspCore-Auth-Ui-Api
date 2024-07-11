using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace AspNetCoreApi.Controllers
{
    [Route(".well-known")]
    [ApiController]
    public class OpenIdController : ControllerBase
    {
        private readonly ILogger<OpenIdController> logger;

        public OpenIdController(ILogger<OpenIdController> logger)
        {
            this.logger = logger;
        }

        public IActionResult OnGet()
        {
            return new JsonResult(new { status = "je suis on get de /openid" });
        }

        [Route("openid-configuration")]
        [HttpGet]
        public IActionResult OnGetOpenIdConfig()
        {
            var openIdConfig = new OpenIdConfiguration
            {
                issuer = "https://localhost:7180", //L'URL de votre serveur OpenID Connect.
                authorization_endpoint = "https://localhost:7180/connect/authorize", // L'URL pour l'authentification (où les utilisateurs se connectent).
                token_endpoint = "https://localhost:7180/connect/token", // L'URL pour échanger le code d'autorisation contre un token d'accès et un token d'identité. 
                userinfo_endpoint = "https://localhost:7180/connect/userinfo", //  L'URL pour obtenir des informations sur l'utilisateur connecté.
                jwks_uri = "https://localhost:7180/.well-known/jwks.json", // L'URL pour obtenir les clés publiques utilisées pour vérifier les tokens JWT.

                response_types_supported = new[] { "code", "token", "id_token", "code id_token", "code token", "id_token token", "code id_token token" },
                subject_types_supported = new[] { "public" },
                id_token_signing_alg_values_supported = new[] { "RS256" },
                scopes_supported = new[] { "openid", "profile", "email", "offline_access" },
                token_endpoint_auth_methods_supported = new[] { "client_secret_basic", "client_secret_post" }
            };

            return new JsonResult(openIdConfig);
        }

        [HttpGet("jwks.json")]
        public IActionResult GetJwks()
        {
            var jwks = new Jwks
            {
                Keys = new List<JwksKey>
            {
                new JwksKey
                {
                    Kty = "RSA",
                    Use = "sig",
                    Kid = Guid.NewGuid().ToString(), // "your-key-id",
                    Alg = "RS256",
                    N = "your-modulus-base64-url-encoded",
                    E = "your-exponent-base64-url-encoded"
                }
            }
            };

            return new JsonResult(jwks);
        }


    }
}
