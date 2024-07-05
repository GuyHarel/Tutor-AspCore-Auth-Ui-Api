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

namespace AspNetCoreRazor.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(string username, string password)
        {
            // Claims
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            // Secret Key
            var secretKey128Bits = "6b9d5e8f3a4b2c1d0e6f7a8b9c0d1e2f3b4c5d6e7f8a9b0c1d2e3f4g5h6i7j8k";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey128Bits));

            // Credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Token
            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JsonResult(new { token = new JwtSecurityTokenHandler().WriteToken(token) });


        }
    }
}
