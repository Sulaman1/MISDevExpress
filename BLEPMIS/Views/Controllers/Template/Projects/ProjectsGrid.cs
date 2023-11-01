using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Projects
{
    [AllowAnonymous]
    public class ProjectsGrid : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("projects-grid")]
        public ActionResult projectsgrid()
        {
            return View();
        }
    }
}
