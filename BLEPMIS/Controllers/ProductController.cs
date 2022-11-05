using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Controllers
{
public class ProductController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
}