using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Dashboard
{
    [AllowAnonymous]
    public class DashboardBlog : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("dashboard-blog")]
        public ActionResult dashboardblog()
        {
            return View();
        }


    }
}
