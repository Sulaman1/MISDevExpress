using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.pages_404
{
    [AllowAnonymous]
    public class PagesStarter : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("pages-starter")]
        public ActionResult pagesstarter()
        {
            return View();
        }
    }
}
