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

        /// <summary>
        /// This endpoing is called when a purchaser changes their payment type. For example a
        /// purchaser may change their credit card from Master Card to American Express. 
        /// </summary>
        /// <param name="cartId">If you want to pass information to your endpoint, you can add the
        /// paramaters to the endpoint url.</param>
        /// <param name="dto">This is the data object received from Apple Pay.</param>
        /// <returns>An Apple Pay Update data object JSON formatted.</returns>
        [HttpPost]
        [Produces("application/json")]
        [Route("/apple-pay/payment-method/{cartId}", Name = "OnPaymentMethodSelected")]
        public async Task<IActionResult> PostAsync(
            string cartId,
            [FromBody] PaymentMethodUpdateDto dto
        ) {

            // validate model
            if (!ModelState.IsValid) return BadRequest();

            // get some of the credit card information supplied
            var currentPaymentData = dto.PaymentMethod;
            var creditCardBrand = currentPaymentData.Network; // this could be null !!!
            var creditCardType = currentPaymentData.Type;
            var currentTotal = decimal.Parse(dto.PaymentMethodUpdate.NewTotal.Amount);

            // if we wanted to give a discount for debit cards we could
            if(creditCardType == PaymentMethodType.debit) {

                // add the discount line items
                dto.PaymentMethodUpdate.NewLineItems.Add(
                    new LineItemDto {
                        Label = "Debit Card Discount",
                        Amount = "-0.50",
                        Type = LineItemType.final
                    }
                );

                // update the total
                dto.PaymentMethodUpdate.NewTotal.Amount = (currentTotal - 0.50m).ToString();

            }
            else {

                // just in case they remove their debit card
                var wasDiscountApplied = dto.PaymentMethodUpdate.NewLineItems.Where(_ => _.Label == "Debit Card Discount").Any();
                dto.PaymentMethodUpdate.NewLineItems.RemoveAll(_ => _.Label == "Debit Card Discount");

                if(wasDiscountApplied) {
                    dto.PaymentMethodUpdate.NewTotal.Amount = (currentTotal + 0.50m).ToString();
                }

            }

            // return updated data
            return Json(new { success = true, update = dto.PaymentMethodUpdate });

        }        

    }

}
