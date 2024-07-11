using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AspNetCoreApi.Controllers
{
    [Route("connect")]
    [ApiController]
    public class OpenIdConnectController : ControllerBase
    {
        private const String secretKey128Bits = "6b9d5e8f3a4b2c1d0e6f7a8b9c0d1e2f3b4c5d6e7f8a9b0c1d2e3f4g5h6i7j8k";

        [Route("token")]
        [HttpPost]
        public IActionResult Token()
        {
            // Valider la requète: client client_id, le client_secret, le grant_type, le code (authorisation code flow)

            // Si valide, retourner un token

            var request = Request.Form;

            // Validate the client_id and client_secret
            var clientId = request["client_id"];
            var clientSecret = request["client_secret"];
            if (!ValidateClient(clientId, clientSecret))
            {
                return BadRequest(new { error = "invalid_client" });
            }

            // Validate the grant_type
            var grantType = request["grant_type"];
            if (grantType != "authorization_code")
            {
                return BadRequest(new { error = "unsupported_grant_type" });
            }

            // Validate the authorization code
            var code = request["code"];
            if (!ValidateAuthorizationCode(code))
            {
                return BadRequest(new { error = "invalid_grant" });
            }

            // Generate the access token
            var token = GenerateToken(clientId);
            var token_id = GenerateIdToken(token);

            // Return the token response
            return Ok(new
            {
                access_token = token,
                token_type = "Bearer",
                expires_in = 3600, // 1 hour expiration time
                id_token = token_id
            });
        }

        private bool ValidateClient(string clientId, string clientSecret)
        {
            // Implement your client validation logic here
            // This is just a simple example
            return clientId == "AspNetCoreApi_clientId" && clientSecret == "AspNetCoreApi_secret";
        }

        private bool ValidateAuthorizationCode(string code)
        {
            // Implement your authorization code validation logic here
            // This is just a simple example
            //return code == "valid_authorization_code";

            return true;
        }

        private string GenerateToken(string clientId)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, clientId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey128Bits));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "your_issuer",
                audience: "your_audience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [Route("authorize")]
        public IActionResult Authorize()
        {
            var queryParameters = Request.Query;

            // Extract query parameters
            var clientId = queryParameters["client_id"].ToString();
            var redirectUri = queryParameters["redirect_uri"].ToString();
            var responseType = queryParameters["response_type"].ToString();
            var scope = queryParameters["scope"].ToString();
            var codeChallenge = queryParameters["code_challenge"].ToString();
            var codeChallengeMethod = queryParameters["code_challenge_method"].ToString();
            var responseMode = queryParameters["response_mode"].ToString();
            var nonce = queryParameters["nonce"].ToString();
            var state = queryParameters["state"].ToString();
            var xClientSKU = queryParameters["x-client-SKU"].ToString();
            var xClientVer = queryParameters["x-client-ver"].ToString();

            // Validate required parameters
            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(redirectUri) || string.IsNullOrEmpty(responseType) ||
                string.IsNullOrEmpty(scope) || string.IsNullOrEmpty(codeChallenge) || string.IsNullOrEmpty(codeChallengeMethod))
            {
                return BadRequest("Missing required query parameters.");
            }

            // TODO: Implement additional validation and processing logic
            // For example, validate client_id, redirect_uri, etc.

            // Generate authorization code
            var authorizationCode = GenerateAuthorizationCode();

            // Construct the response URL
            var responseUrl = $"{redirectUri}?code={authorizationCode}&state={state}";

            return new RedirectResult(responseUrl);

        }

        private string GenerateAuthorizationCode()
        {
            // TODO: Implement your authorization code generation logic
            return Guid.NewGuid().ToString("N");
        }

        private string GenerateIdToken(string clientId)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, clientId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            // Add more claims as needed
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey128Bits));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7180",
                audience: "AspNetCoreApi_clientId",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
