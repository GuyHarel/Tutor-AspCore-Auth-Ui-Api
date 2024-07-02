using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AspNetCoreRazor.Security.Authentication
{
    public class BookAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ILogger<BookAuthenticationHandler> logger;

        public BookAuthenticationHandler(Microsoft.Extensions.Options.IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
            this.logger = logger.CreateLogger<BookAuthenticationHandler>();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            logger.LogInformation("HandleAuthenticateAsync");
            await Task.Delay(1);

            var result = Success();
            //var result = Fail();  // va appeler Challenge
            //var result = NoResult(); // va appeler Challenge

            return result;

        }


        /*
         
        Un défi d'authentification est émis lorsque l'utilisateur tente d'accéder à une ressource protégée sans être authentifié
        la méthode HandleChallengeAsync permet de renvoyer une réponse spécifique qui incite le client à fournir les informations d'identification nécessaires.
        */

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            // Retourner le HTTP 401 de base
            //return base.HandleChallengeAsync(properties);

            // Retourner 200, mais n'affiche pas la page
            //Response.StatusCode = 200;
            //return Task.CompletedTask;

            // Retourner un JSON
            //Response.StatusCode = 401;
            //await Results.Json(new {abc = 123}).ExecuteAsync(Context);

            //Response.Redirect("/login");

            //throw new ApplicationException("ceci est un test");

            //await Context.ForbidAsync(Scheme.Name, properties);

            await base.HandleChallengeAsync(properties);

            //await Task.CompletedTask;

            /*
             * Si on est un API, on peut retourner
             * 
             * 
             
            
            Response.Headers["WWW-Authenticate"] = "Basic realm=\"zoneentreprise-cmmtq-test-api.azurewebsites.net\"";


            Pour une API, il est effectivement courant d'inclure l'en-tête WWW-Authenticate dans la réponse 401 pour indiquer au client quel type 
            d'authentification est requis. Cet en-tête est particulièrement utile pour les clients HTTP automatisés (comme les navigateurs et 
            les clients d'API) qui peuvent réagir en demandant des informations d'identification à l'utilisateur.

            */

        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {

            await base.HandleForbiddenAsync(properties);

        }

        private AuthenticateResult Success()
        {
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, "alfred"),
                new Claim(ClaimTypes.Name, "tremblay"),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);

        }

        private AuthenticateResult Fail()
        {
            var result = AuthenticateResult.Fail("échec de BookAuthenticationHandler");

            return result;
        }

        private AuthenticateResult NoResult()
        {
            var result = AuthenticateResult.NoResult();

            return result;
        }


        private AuthenticateResult HandleAuthenticateAsyncTest()
        {
            var resultFail = AuthenticateResult.Fail("échec de BookAuthenticationHandler");
            var resultNoResult = AuthenticateResult.NoResult();
            var resultOk = AuthenticateResult.Success(new AuthenticationTicket(new System.Security.Claims.ClaimsPrincipal(), "unNom"));

            var result = resultOk;


            return result;
        }
    }
}
