using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Tables
{
    [AllowAnonymous]
    public class TablesEditable : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("tables-editable")]
        public ActionResult tableseditable()
        {
            return View();
        }
    }
}
