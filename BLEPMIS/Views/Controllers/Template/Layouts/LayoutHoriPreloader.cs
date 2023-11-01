using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BLEPMIS.Layouts
{
    [AllowAnonymous]
    public class LayoutHoriPreloader : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
