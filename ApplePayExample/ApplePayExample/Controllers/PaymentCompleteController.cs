﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplePayExample.Controllers {

    public class PaymentCompleteController : Controller {

        [Route("/payment-complete")]
        public IActionResult Index() {

            return View();

        }

    }

}
