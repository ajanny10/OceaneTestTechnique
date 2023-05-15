using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Test_Technique.Models;

namespace Test_Technique.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{StatusCode}")]
        [AllowAnonymous]
        public IActionResult Index(int StatusCode)
        {

            StatusResult model = new StatusResult();
            var statusCoderesult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (StatusCode)
            {
                case 404:
                    {
                        model.Message  = "Désolé, la ressource demandée est introuvable";
                        model.Path     = statusCoderesult.OriginalPath ;
                        model.Qs       = statusCoderesult.OriginalQueryString;
                    }
                    break;
            }

            return View("NotFound",model);
        }
    }
}
