using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.pages_404
{
    [AllowAnonymous]
    public class PagesFaqs : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("pages-faqs")]
        public ActionResult pagesfaqs()
        {
            return View();
        }
    }
}
