﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLEPMIS.Auth
{
    [AllowAnonymous]
    public class ConfirmMail : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
