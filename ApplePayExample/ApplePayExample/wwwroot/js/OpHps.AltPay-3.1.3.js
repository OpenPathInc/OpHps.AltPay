
var OpHps = {

    // GLOBAL ALTERNATIVE PAYMENT ATTRIBUTES
    // ================================================================================
    ApiLoginId: function (apiLoginId) {
        this.apiLoginId = apiLoginId;
    },
    PaymentCompleteReturnUrl: function (paymentCompleteReturnUrl) {
        this.paymentCompleteReturnUrl = paymentCompleteReturnUrl;
    },
    EnableLogging: function (enabled) {
        this.loggingEnabled = enabled;
    },
    AttachLog: function (idOrClass) {
        this.logElement = idOrClass;
        OpHps.Log("Logs attached to " + idOrClass + " element.");
    },
    // default attributes
    // --------------------------------------------------------------------------------
    apiLoginId: "UNKNOWN",
    paymentCompleteReturnUrl: "/payment-complete",
    loggingEnabled: false,
    logElement: ".logging",
    //server: "https://op-hps-apple-pay-server.ngrok.io/v3/apple-pay",
    server: "https://api.openpath.io/platform/v3/apple-pay",

    // APPLE PAY FUNCTIONS
    // ================================================================================
    ApplePay: {

        // ATTACHED ELEMENTS
        // ================================================================================
        AttachApplePayButton: function (idOrClass) {
            this.applePayButtonElement = idOrClass;
            OpHps.Log("Apple Pay button attached to " + idOrClass + " element.");
        },
        // default element attributes
        // --------------------------------------------------------------------------------
        applePayButtonElement: ".apple-pay-button",

        // ATTACHED ENDPOINTS
        // ================================================================================
        AttachPaymentMethodSelectedEndpoint: function (endpoint) {
            this.paymentMethodSelectedEndpoint = endpoint;
        },
        AttachShippingMethodSelectedEndpoint: function (endpoint) {
            this.shippingMethodSelectedEndpoint = endpoint;
        },
        AttachShippingContactSelectedEndpoint: function (endpoint) {
            this.shippingContactSelectedEndpoint = endpoint;
        },
        // default endpoint attributes
        // --------------------------------------------------------------------------------
        paymentMethodSelectedEndpoint: "/apple-pay/payment-method",
        shippingMethodSelectedEndpoint: "/apple-pay/shipping-method",
        shippingContactSelectedEndpoint: "/apple-pay/shipping-contact",


        // APPLE PAY BUTTON FUNCTIONS
        // ================================================================================
        ShowApplePayButton: function () {

            OpHps.Log("Showing Apple Pay Button");

            try {

                if (window.ApplePaySession && ApplePaySession.canMakePayments()) {

                    var button = $(this.applePayButtonElement);
                    var language = $("html").attr("lang") || "en";
                    var buttonStyle = "-webkit-appearance: -apple-pay-button;" +
                        "-apple-pay-button-type: plain;" +
                        "-apple-pay-button-style: black;";

                    button.attr("lang", language);
                    button.on("click", OpHps.ApplePay.BeginPayment);

                    if ("openPaymentSetup" in ApplePaySession) {

                        button.attr("style", buttonStyle);

                    }
                    else {

                        button.attr("style", buttonStyle);

                    }

                    $(this.applePayButtonElement).show();

                }
                else {

                    $(this.applePayButtonElement).hide();

                }

            }
            catch (err) {
                OpHps.Log(err.message);
            }

        },

        // APPLE PAY PAYMENT FUNCTIONS
        // ================================================================================
        BeginPayment: function (e) {

            e.preventDefault();

            OpHps.Log("Apple Pay Button Clicked");

            try {

                // create the apple pay session
                var session = new ApplePaySession(6, OpHps.ApplePay.ApplePayPaymentRequest());


                // validate the merchant and create the payment information
                // --------------------------------------------------------------------------------
                session.onvalidatemerchant = function (event) {

                    OpHps.Log("Validating Merchant");

                    try {

                        // create the payload
                        var data = {
                            validationUrl: event.validationURL
                        };

                        // setup antiforgery http header
                        var antiforgeryHeader = $("meta[name='x-antiforgery-name']").attr("content");
                        var antiforgeryToken = $("meta[name='x-antiforgery-token']").attr("content");
                        var headers = {};

                        headers[antiforgeryHeader] = antiforgeryToken;

                        OpHps.Log("Calling endpoint");
                        OpHps.Log(OpHps.server + "/hps-validate-merchant/" + OpHps.apiLoginId);
                        OpHps.Log(JSON.stringify(data));

                        // post the payload to the server to validate the merchant
                        // session using the merchant certificate
                        $.ajax({
                            url: OpHps.server + "/hps-validate-merchant/" + OpHps.apiLoginId,
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(data),
                            headers: headers
                        }).then(function (data) {

                            OpHps.Log("Received data....");

                            if (data.success) {

                                // complete validation by passing the merchant session to the apple pay session
                                session.completeMerchantValidation(data.merchantSession);

                            }
                            else {

                                OpHps.Log(data.error);

                            }

                        });

                    }
                    catch (err) {
                        OpHps.Log(err.message);
                    }

                };


                // what to do when the payment method is changed
                // --------------------------------------------------------------------------------
                session.onpaymentmethodselected = function (event) {

                    OpHps.Log("Payment Method Selected");

                    try {

                        var update = {};
                        var postData = {
                            paymentMethod: event.paymentMethod,
                            update: {
                                newTotal: OpHps.ApplePay.total,
                                newLineItems: OpHps.ApplePay.paymentLineItems
                            }
                        };

                        // setup antiforgery http header
                        var antiforgeryHeader = $("meta[name='x-antiforgery-name']").attr("content");
                        var antiforgeryToken = $("meta[name='x-antiforgery-token']").attr("content");
                        var headers = {};

                        headers[antiforgeryHeader] = antiforgeryToken;

                        // post the payment
                        $.ajax({
                            url: OpHps.ApplePay.paymentMethodSelectedEndpoint,
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(postData),
                            async: true,
                            headers: headers,
                            complete: function (data) {

                                if (data.responseText === '') return;

                                var response = JSON.parse(data.responseText);

                                if (response.success) {

                                    session.completePaymentMethodSelection(response.update);

                                }
                                else {

                                    update = {
                                        newTotal: {
                                            label: OpHps.ApplePay.total.label,
                                            amount: OpHps.ApplePay.total.amount,
                                            type: OpHps.ApplePay.total.type
                                        }
                                    };

                                }

                            }

                        });

                    } catch (err) {
                        OpHps.Log(err.message);
                    }

                };


                // what to do when the shipping information is changed
                // --------------------------------------------------------------------------------
                session.onshippingcontactselected = function (event) {

                    // LOGGING
                    OpHps.Log("Shipping Contact Selected");

                    try {

                        var update = {};
                        var postData = {
                            shippingContact: event.shippingContact,
                            update: {
                                newTotal: OpHps.ApplePay.total,
                                newLineItems: OpHps.ApplePay.paymentLineItems
                            }
                        };

                        // setup antiforgery http header
                        var antiforgeryHeader = $("meta[name='x-antiforgery-name']").attr("content");
                        var antiforgeryToken = $("meta[name='x-antiforgery-token']").attr("content");
                        var headers = {};

                        headers[antiforgeryHeader] = antiforgeryToken;

                        // post the payment
                        $.ajax({
                            url: OpHps.ApplePay.shippingContactSelectedEndpoint,
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(postData),
                            async: true,
                            headers: headers,
                            complete: function (data) {

                                var response = JSON.parse(data.responseText);

                                if (response.success) {

                                    session.completeShippingContactSelection(response.update);

                                }
                                else {

                                    update = {
                                        newTotal: {
                                            label: OpHps.ApplePay.total.label,
                                            amount: OpHps.ApplePay.total.amount,
                                            type: OpHps.ApplePay.total.type
                                        }
                                    };

                                }

                            }

                        });

                    } catch (err) {
                        OpHps.Log(err.message);
                    }

                };


                // what to do when the shipping method is changed
                // --------------------------------------------------------------------------------
                session.onshippingmethodselected = function (event) {

                    OpHps.Log("Shipping Method Selected");

                    try {

                        var update = {};
                        var postData = {
                            paymentMethod: event.shippingMethod,
                            update: {
                                newTotal: OpHps.ApplePay.total,
                                newLineItems: OpHps.ApplePay.paymentLineItems
                            }
                        };

                        // setup antiforgery http header
                        var antiforgeryHeader = $("meta[name='x-antiforgery-name']").attr("content");
                        var antiforgeryToken = $("meta[name='x-antiforgery-token']").attr("content");
                        var headers = {};

                        headers[antiforgeryHeader] = antiforgeryToken;

                        // post the payment
                        $.ajax({
                            url: OpHps.ApplePay.shippingMethodSelectedEndpoint,
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(postData),
                            async: true,
                            headers: headers,
                            complete: function (data) {

                                var response = JSON.parse(data.responseText);

                                if (response.success) {

                                    session.completeShippingMethodSelection(response.update);

                                }
                                else {

                                    update = {
                                        newTotal: {
                                            label: OpHps.ApplePay.total.label,
                                            amount: OpHps.ApplePay.total.amount,
                                            type: OpHps.ApplePay.total.type
                                        }
                                    };

                                }

                            }

                        });

                    } catch (err) {
                        OpHps.Log(err.message);
                    }

                };


                // what to do when the payment has been authorized
                // --------------------------------------------------------------------------------
                session.onpaymentauthorized = function (event) {

                    OpHps.Log("Payment Authorized");
                    OpHps.Log("payment");
                    OpHps.Log(JSON.stringify(event.payment));
                    OpHps.Log("payment.token.paymentData");
                    OpHps.Log(JSON.stringify(event.payment.token.paymentData));

                    try {

                        var encodedUrl = encodeURIComponent(OpHps.paymentCompleteReturnUrl);

                        OpHps.Log("URL Encoding");
                        OpHps.Log(OpHps.paymentCompleteReturnUrl);
                        OpHps.Log(encodedUrl);

                        // post the payment to openpath
                        $.ajax({
                            url: OpHps.server + '/payment-authorized/' + OpHps.apiLoginId + '/' + encodedUrl,
                            method: "POST",
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(event.payment)
                        }).then(function (data) {

                            if (data !== '') {

                                session.completePayment({
                                    status: ApplePaySession.STATUS_SUCCESS,
                                    errors: []
                                });

                                window.location.href = data;

                            }
                            else {

                                session.completePayment({
                                    status: ApplePaySession.STATUS_FAILURE,
                                    errors: []
                                });

                            }

                        });


                    } catch (err) {
                        OpHps.Log(err.message);
                    }

                };

                // start the session to display the apple pay sheet
                session.begin();

            }
            catch (err) {
                OpHps.Log(err.message);
            }

        },

        // apple pay payment request functions
        // --------------------------------------------------------------------------------

        // get the payment information
        ApplePayPaymentRequest: function () {

            var paymentRequest = {
                countryCode: this.countryCode,
                currencyCode: this.currencyCode,
                merchantCapabilities: ["supports3DS"],
                supportedNetworks: this.supportedNetworks,
                requiredShippingContactFields: this.requiredShippingContactFields,
                shippingMethods: this.shippingMethods,
                total: this.total
            };

            return paymentRequest;

        },

        // set the country
        SetCountryCode: function (country) {

            this.countryCode = country;

            switch (country) {
                case "US": this.currencyCode = "USD"; break;
                case "GB": this.currencyCode = "GBP"; break;
            }

            OpHps.Log("Currency set to " + this.currencyCode + ".");

        },

        // set the supported payment methods
        Visa: function (supported) { this.SupportNetwork("visa", supported); },
        MasterCard: function (supported) { this.SupportNetwork("masterCard", supported); },
        AmericanExpress: function (supported) { this.SupportNetwork("amex", supported); },
        Discover: function (supported) { this.SupportNetwork("discover", supported); },

        SupportNetwork: function (network, supported) {
            if (supported && !this.supportedNetworks.includes(network)) {
                this.supportedNetworks.push(network);
            }
            if (!supported && this.supportedNetworks.includes(network)) {
                this.supportedNetworks.splice(this.supportedNetworks.indexOf(network), 1);
            }
        },

        // set the total
        SetTotal: function (label, amount) {

            this.total.label = label;
            this.total.amount = amount;

        },

        // line items
        ClearLineItems: function () {
            this.paymentLineItems = [];
        },
        AddLineItem: function (label, amount) {
            this.paymentLineItems.push({
                label: label,
                type: "final",
                amount: amount
            });
        },

        // base payment attributes
        countryCode: "US",
        currencyCode: "USD",
        merchantCapabilities: ["supports3DS"],
        supportedNetworks: [],
        total: {
            label: "Total",
            type: "final",
            amount: 0.00
        },
        paymentLineItems: [
            {
                label: "Subtotal",
                amount: 0.00,
                type: "final"
            }
        ],

        // SHIPPING METHODS
        // ================================================================================
        ClearShippingMethod: function () {
            this.shippingMethods = [];
        },
        AddShippingMethod: function (label, amount, identifier, detail) {
            this.shippingMethods.push({ label: label, amount: amount, identifier: identifier, detail: detail });
        },

        // shipping method attributes
        shippingMethods: [],

        // SHIPPING FIELDS
        // ================================================================================
        RequireShippingContactEmail: function (supported) { this.RequiredShippingContactFields("email", supported); },
        RequireShippingContactName: function (supported) { this.RequiredShippingContactFields("name", supported); },
        RequireShippingContactPhone: function (supported) { this.RequiredShippingContactFields("phone", supported); },
        RequireShippingContactAddress: function (supported) { this.RequiredShippingContactFields("postalAddress", supported); },

        RequiredShippingContactFields: function (field, required) {
            if (required && !this.requiredShippingContactFields.includes(field)) {
                this.requiredShippingContactFields.push(field);
            }
            if (!required && this.requiredShippingContactFields.includes(field)) {
                this.requiredShippingContactFields.splice(this.supportedNetworks.indexOf(field), 1);
            }
        },

        // default shipping field attributes
        requiredShippingContactFields: ["email", "name", "phone", "postalAddress"]

    },

    // GOOGLE PAY FUNCTIONS
    // ================================================================================
    GooglePay: {
        // future implementation
    },

    // HELPER FUNCTIONS
    // ================================================================================
    Log: function (message) {

        if (OpHps.loggingEnabled) {

            console.log(message);
            var html = $(OpHps.logElement).html() + message + "<br />";
            $(OpHps.logElement).html(html);

        }

    }

};