using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Charts
{
    [AllowAnonymous]
    public class ChartsTui : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("charts-tui")]
        public ActionResult chartstui()
        {
            return View();
        }
    }
}
