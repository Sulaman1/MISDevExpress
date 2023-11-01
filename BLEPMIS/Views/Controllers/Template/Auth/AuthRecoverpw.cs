using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Auth
{
    [AllowAnonymous]
    public class AuthRecoverpw : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("auth-recoverpw")]
        public ActionResult authrecoverpw()
        {
            return View();
        }
    }
}
