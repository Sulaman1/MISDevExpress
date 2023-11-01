using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Tasks
{
    [AllowAnonymous]
    public class TasksCreate : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("tasks-create")]
        public ActionResult taskscreate()
        {
            return View();
        }
    }
}
