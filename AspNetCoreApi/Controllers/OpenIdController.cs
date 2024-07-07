using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace AspNetCoreApi.Controllers
{
    [Route("/.well-known")]
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
        public IActionResult OnGetOpenIdConfig()
        {
            logger.LogInformation("un test de log");
            return new JsonResult(new { status = "je suis on OnGetOpenIdConfig" });
        }
    }
}
