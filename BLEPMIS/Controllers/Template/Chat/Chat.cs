using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Chat
{
    [AllowAnonymous]
    public class Chat : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
