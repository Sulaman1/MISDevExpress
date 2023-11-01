using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.UI
{
    [AllowAnonymous]
    public class UiCarousel : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("ui-carousel")]
        public ActionResult uicarousel()
        {
            return View();
        }
    }
}
