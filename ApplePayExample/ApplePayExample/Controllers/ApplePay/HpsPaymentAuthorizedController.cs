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
    public class HpsPaymentAuthorizedController : Controller {

        public HpsPaymentAuthorizedController() {

        }

        [HttpPost]
        [Produces("application/json")]
        [Route("/v3/apple-pay/payment-authorized/{apiLoginId}", Name = "OnPaymentAuthorized")]
        public async Task<IActionResult> PostAsync(
            string apiLoginId,
            [FromBody] PaymentDto model,
            CancellationToken cancellationToken = default
        ) {
            
            if (!ModelState.IsValid) return BadRequest();

            // finalize the payment through the processor
            var return_url = "Payment Successful"; // await Open.Path.Api.Shopify.Helper.OpenPathPostHelper.Process(checkoutContainer, model, ipAddress);

            // Return the merchant session as-is to the JavaScript as JSON.
            return Json(return_url);

        }
    }
}
