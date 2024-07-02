using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace AspNetCoreRazor.Security.Authentication
{
    public class BookCookieAuthenticationHandler : CookieAuthenticationHandler
    {
        public BookCookieAuthenticationHandler(IOptionsMonitor<CookieAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder) 
            : base(options, logger, encoder)
        {

        }
    }
}
