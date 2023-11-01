using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Dashboard
{
    [AllowAnonymous]
    public class DashboardCrypto : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("dashboard-crypto")]
        public ActionResult dashboardcrypto()
        {
            return View();
        }


    }
}
