using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Icon
{
    [AllowAnonymous]
    public class IconsMaterialDesign : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("icons-materialdesign")]
        public ActionResult iconsmaterialdesign()
        {
            return View();
        }
    }
}
