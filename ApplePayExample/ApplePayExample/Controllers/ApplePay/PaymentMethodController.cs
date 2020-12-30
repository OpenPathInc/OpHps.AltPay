using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenPath.Dto.ApplePay;
using OpenPath.Dto.ApplePay.Enumerator;

namespace ApplePayExample.Controllers.ApplePay {

    [EnableCors("HPS")]
    [Route("apple-pay/[controller]")]
    public class PaymentMethodController : Controller {

        public PaymentMethodController() {

        }

        [HttpPost]
        [Produces("application/json")]
        [Route("/apple-pay/payment-method/{cartId}", Name = "OnPaymentMethodSelected")]
        public async Task<IActionResult> PostAsync(
            string cartId,
            [FromBody] PaymentMethodUpdateDto model
        ) {

            // validate model
            if (!ModelState.IsValid) return BadRequest();

            // make sure we don't pass back any nulls
            foreach(var lineItem in model.PaymentMethodUpdate.NewLineItems) {

                if(lineItem.Amount == null) lineItem.Amount = "0.00";

            }

            // Return the merchant session as-is to the JavaScript as JSON.
            return Json(new { success = true, update = model.PaymentMethodUpdate });

        }        

    }

}
