using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Controllers
{
    public class ReportCreatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
