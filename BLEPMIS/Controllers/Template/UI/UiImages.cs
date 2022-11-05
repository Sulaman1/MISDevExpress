using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.UI
{
    [AllowAnonymous]
    public class UiImages : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("ui-images")]
        public ActionResult uiimages()
        {
            return View();
        }
    }
}
