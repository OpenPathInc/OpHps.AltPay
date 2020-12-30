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
    public class ShippingMethodController : Controller {

        public ShippingMethodController() {

        }

        [HttpPost]
        [Produces("application/json")]
        [Route("/apple-pay/shipping-method/{cartId}", Name = "OnShippingMethodSelected")]
        public async Task<IActionResult> PostAsync(
            string cartId,
            [FromBody] ShippingMethodUpdateDto model
        ) {

            if (!ModelState.IsValid) return BadRequest();
                
            // update the line items
            model.ShippingMethodUpdate.NewLineItems =  new List<LineItemDto> {
                new LineItemDto {
                    Label = "Subtotal",
                    Type = LineItemType.final,
                    Amount = "20.00"
                },
                new LineItemDto() {
                    Label = "Discount",
                    Type = LineItemType.final,
                    Amount = "2.00"
                },
                new LineItemDto() {
                    Label = "Shipping",
                    Type = LineItemType.final,
                    Amount = "10.00"
                },
                new LineItemDto() {
                    Label = "Taxes",
                    Type = LineItemType.final,
                    Amount = "3.00"
                }
            };

            // update the total
            model.ShippingMethodUpdate.NewTotal = new LineItemDto() {
                Label = "Total",
                Type = LineItemType.final,
                Amount = "35.00"
            };

            // Return the merchant session as-is to the JavaScript as JSON.
            return Json(new { success = true, update = model.ShippingMethodUpdate });

        }

    }

}
