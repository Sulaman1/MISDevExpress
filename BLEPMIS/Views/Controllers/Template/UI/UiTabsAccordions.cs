using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.UI
{
    [AllowAnonymous]
    public class UiTabsAccordions : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("ui-tabs-accordions")]
        public ActionResult uitabsaccordions()
        {
            return View();
        }
    }
}
