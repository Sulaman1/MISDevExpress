using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BLEPMIS.Crypto
{
    [AllowAnonymous]
    public class CryptoWallet : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("crypto-wallet")]
        public ActionResult cryptowallet()
        {
            return View();
        }
    }
}
