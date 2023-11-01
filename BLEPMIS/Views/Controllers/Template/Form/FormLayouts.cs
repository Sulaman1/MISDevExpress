using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Form
{
    [AllowAnonymous]
    public class FormLayouts : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("form-layouts")]
        public ActionResult formlayouts()
        {
            return View();
        }
    }
}
