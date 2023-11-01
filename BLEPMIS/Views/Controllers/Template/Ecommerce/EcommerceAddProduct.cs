using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Ecommerce
{
    [AllowAnonymous]
    public class EcommerceAddProduct : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("ecommerce-addproduct")]
        public ActionResult ecommerceaddproduct()
        {
            return View();
        }
    }
}
