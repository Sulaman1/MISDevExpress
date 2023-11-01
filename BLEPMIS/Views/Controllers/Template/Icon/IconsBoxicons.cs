using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Icon
{
    [AllowAnonymous]
    public class IconsBoxicons : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("icons-boxicons")]
        public ActionResult iconsboxicons()
        {
            return View();
        }
    }
}
