using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Layouts
{
    [AllowAnonymous]
    public class LayoutsHorizontal : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("layout-horizontal")]
        public ActionResult layouthorizontal()
        {
            TempData["ModeName"] = Contants.LAYOUT_HORIZONTAL;
            TempData["WelcomeText"] = "LAYOUT_HORIZONTAL";
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
