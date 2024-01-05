using Microsoft.AspNetCore.Mvc;

namespace AsterWebApp.Areas.Report.Controllers
{
    [Area("Report")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
