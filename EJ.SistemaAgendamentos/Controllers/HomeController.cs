using Microsoft.AspNetCore.Mvc;

namespace EJ.SistemaAgendamentos.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            if(HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated)
                return View();
            else {
                return RedirectToAction("Login", "Auth");
            }
        }

    }
}
