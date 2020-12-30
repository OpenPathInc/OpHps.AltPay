using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OpenPath.Dto.ApplePay;

namespace ApplePayExample.Controllers.ApplePay
{

    [EnableCors("HPS")]
    [Route("v3/[controller]")]
    public class HpsValidateMerchantController : Controller {

        public HpsValidateMerchantController() {

        }

        [HttpPost]
        [Produces("application/json")]
        [Route("/v3/apple-pay/hps-validate-merchant/{apiKey}")]
        public async Task<IActionResult> PostAsync(
            string apiKey,
            [FromBody] ValidateMerchantSessionDto validateMerchantSessionDto,
            CancellationToken cancellationToken = default
        ) {

            // define values
            var certificateThumbPrint = "c97a696a5d4ac7055fe101e7882138a5153cbb28";
            var domainName = "op-hps-apple-pay-1.ngrok.io";
            var storeName = "merchant.openpath.io.corp";

            try { 

                // you may wish to additionally validate that the uri specified for
                // merchant validation in the request body is a documented apple
                // pay js hostname. the ip addresses and dns hostnames of these
                // servers are available here:
                // https://developer.apple.com/documentation/applepayjs/setting_up_server_requirements
                if (!ModelState.IsValid ||
                    string.IsNullOrWhiteSpace(validateMerchantSessionDto.ValidationUrl) ||
                    !Uri.TryCreate(validateMerchantSessionDto.ValidationUrl, UriKind.Absolute, out Uri requestUri)
                ) {
                    return BadRequest();
                }

                // create the json payload to post to the apple pay merchant
                // validation url
                var request = new MerchantSessionRequestDto {
                    MerchantIdentifier  = loadCertificateIdentifier(certificateThumbPrint),
                    DomainName          = domainName,
                    DisplayName         = domainName
                };

            
                var merchantSession = (JObject)null;
                var httpClientHandler = new HttpClientHandler();
                    httpClientHandler.ClientCertificates.Add(loadCertificate(certificateThumbPrint));
                    httpClientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;

                using (var httpClient = new HttpClient(httpClientHandler))
                {

                    using (var response = await httpClient.PostAsJsonAsync(requestUri, request, cancellationToken))
                    {

                        response.EnsureSuccessStatusCode();
                        merchantSession = await response.Content.ReadAsAsync<JObject>(cancellationToken);

                    }

                }

                // Return the merchant session as-is to the JavaScript as JSON.
                return Json(new { success = true, merchantSession, error = "" });

            }
            catch(Exception ex) {

                return Json(new { success = false, merchantSession = "", error = ex.ToString() });

            }

        }

        #region == HELPERS =========================================================================================

            private X509Certificate2 loadCertificate(string thumbprint) {

                // Load the certificate from the current user's certificate store. This
                // is useful if you do not want to publish the merchant certificate with
                // your application, but it is also required to be able to use an X.509
                // certificate with a private key if the user profile is not available,
                // such as when using IIS hosting in an environment such as Microsoft Azure.

                var certificate = (X509Certificate2)null;

                using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine)) {

                    store.Open(OpenFlags.ReadOnly);

                    // strip any non-hexadecimal values and make uppercase
                    thumbprint = Regex.Replace(thumbprint.Trim(), @"[^\da-fA-F]", string.Empty).ToUpper();

                    var certificates = store.Certificates.Find(
                        X509FindType.FindByThumbprint,
                        thumbprint,
                        validOnly: false);

                    if (certificates.Count < 1) {
                        throw new InvalidOperationException(
                            $"Could not find Apple Pay merchant certificate with thumbprint '{thumbprint}' from store '{store.Name}' in location '{store.Location}'.");
                    }

                    certificate = certificates[0];

                }

                // Convert the raw ASN.1 data to a string containing the ID
                return certificate;

            }

            private string loadCertificateIdentifier(string thumbprint) {

                var certificate = loadCertificate(thumbprint);

                // this oid returns the asn.1 encoded merchant identifier
                var extension = certificate.Extensions["1.2.840.113635.100.6.32"];

                if (extension == null) {
                    return string.Empty;
                }

                // Convert the raw ASN.1 data to a string containing the ID
                return Encoding.ASCII.GetString(extension.RawData).Substring(2);

            }

        #endregion

    }
}
