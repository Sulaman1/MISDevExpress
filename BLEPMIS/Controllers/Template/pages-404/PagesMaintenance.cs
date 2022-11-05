using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.pages_404
{
    [AllowAnonymous]
    public class PagesMaintenance : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("pages-maintenance")]
        public ActionResult pagesmaintenance()
        {
            return View();
        }
    }
}
