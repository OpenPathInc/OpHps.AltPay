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
    public class ShippingContactController : Controller {

        public ShippingContactController() {

        }


        [HttpPost]
        [Produces("application/json")]
        [Route("/apple-pay/shipping-contact/{cartId}", Name = "OnShippingContactSelected")]
        public async Task<IActionResult> PostAsync(
            string cartId,
            [FromBody] ShippingContactUpdateDto model
        ) {

            // validate
            if (!ModelState.IsValid) return BadRequest();

            // initialize shipping methods
            if(model.ShippingContactUpdate.NewShippingMethods == null) model.ShippingContactUpdate.NewShippingMethods = new List<ShippingMethodDto>();
            if(model.ShippingContactUpdate.Errors             == null) model.ShippingContactUpdate.Errors             = new List<ErrorDto>();

            // update the shipping methods
            model.ShippingContactUpdate.NewShippingMethods.Add(
                new ShippingMethodDto() {
                    Amount      = "10.00",
                    Detail      = $"Arives {DateTime.Now.AddDays(1).ToLongDateString()}",
                    Identifier  = "NDS-10",
                    Label       = "Next Day Shipping"
                }
            );

            // update the line items
            model.ShippingContactUpdate.NewLineItems = new List<LineItemDto> {
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
            model.ShippingContactUpdate.NewTotal = new LineItemDto() {
                Label   = "Total",
                Type    = LineItemType.final,
                Amount  = "35.00"
            };

            // Return the merchant session as-is to the JavaScript as JSON.
            return Json(new { success = true, update = model.ShippingContactUpdate });

        }

    }

}
