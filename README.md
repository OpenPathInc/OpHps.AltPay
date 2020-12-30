
# OpenPath Heartland Payment Systems Alternative Payment Methods
Alternative Payment Methods (Apple Pay and Google Pay) for Heartland Payment Systems

## Quick Start
To be created!

## OpHps.AltPay.js
### ApiLoginId
This is your unique key that associates your Apple Pay account with OpenPath and Heartland System, which can be found under sites in the OpenPath backoffice. https://client.openpath.io. For testing purposes, we have provided 10 varified ngrok URLs and matching API Login IDs, which can be found below.

OpHps.ApiLoginId(API Login ID)
```
OpHps.ApiLoginId('XXXXXXXXXXXX');
```

### PaymentCompleteReturnUrl
If the payment is successful this is the URL the server will redirect to with the details of the transaction in the query string.

OpHps.PaymentCompleteReturnUrl(Redirect URL)
```
OpHps.PaymentCompleteReturnUrl('/payment-complete);
```

#### Returns the following paramaters
TransactionId
PacketId
```
/payment-complete?TransactionId=4928399223&PacketId=4949223
```

### EnableLogging
Enable logging turns on the debugging output to the browsers console and to an HTML element if assigned.

OpHps.EnableLogging(true|false)
```
OpHps.EnableLogging(true);
```

### Log
Will output messages to the logging service.

OpHps.Log(Message)
```
OpHps.Log('Hey something happened here!');
```

### ApplePay.AttachApplePayButton
In order to display the Apple Pay button your HTML must have a element that we can attach the button to, use this function to attach that elements Class or ID.

OpHps.ApplePay.AttachApplePayButton(Element Class or ID)
```
OpHps.ApplePay.AttachApplePayButton('.apple-pay-button');
```

#### Example HTML Element
```
<span class="apple-pay-button hidden"></span>
```

### ApplePay.AttachLog
If you want the browsers console output from the logger to also be updated on page, which can be useful when testing on mobile devices, you can attach the logger to an HTML element.

OpHps.ApplePay.AttachLog(Element Class or ID)
```
OpHps.ApplePay.AttachLog('#log-output');
```
#### Example HTML Element
```
<div id="log-output"></div>
```

### Attaching Endpoints
Once the Apple Pay button has been displayed and a user clicks on that button, the Apple Pay payment pane will be displayed to the user. In this pane, the user has the ability to change their shipping address, payment types and contact information, as well as choice a shipping method. When these items are changed, the corresponding endpoints will be called below. These endpoints will send the data that was changed and give you the ability to modify the pricing and shipping options available in the pane based on these changes. These endpoints must be available on your server and you must provide the updates.

### ApplePay.AttachPaymentMethodSelectedEndpoint
When a user changes their payment type this attached endpoint will be posted to.

OpHps.ApplePay.AttachPaymentMethodSelectedEndpoint(Endpoint)
```
OpHps.ApplePay.AttachPaymentMethodSelectedEndpoint('/apple-pay/payment-method/' + cartId);
```

### ApplePay.AttachShippingMethodSelectedEndpoint
When a user changes their shipping method this attached endpoint will be posted to.

OpHps.ApplePay.AttachShippingMethodSelectedEndpoint(Endpoint)
```
OpHps.ApplePay.AttachShippingMethodSelectedEndpoint('/apple-pay/shipping-method/' + cartId);
```

### ApplePay.AttachShippingContactSelectedEndpoint
When a user changes their contact information this attached endpoint will be posted to.

OpHps.ApplePay.AttachShippingContactSelectedEndpoint(Endpoint)
```
OpHps.ApplePay.AttachShippingContactSelectedEndpoint('/apple-pay/shipping-contact/' + cartId);
```

### ApplePay.ShowApplePayButton
Once you've completed the initial configuration of Apple Pay, the final is to execute the Show Apple Pay Button function.

OpHps.ApplePay.ShowApplePayButton()

```
OpHps.ApplePay.ShowApplePayButton();
```

### ApplePay.SetCountryCode
To change the default currency of USD, you can set the country code, which in turn will change the currency.

OpHps.ApplePay.SetCountryCode(Two letter country code)

```
OpHps.ApplePay.SetCountryCode('GB');
```

#### Examples
| Country| Country Code | Currency Code |
| ----------- | ----------- | -----------|
| United States | US | USD  |
| Great Britain  | GB | GBP |

### ApplePay.Visa
Tells Apple Pay to accept or not accept Visa cards.

OpHps.ApplePay.Visa(true | false)

```
OpHps.ApplePay.Visa(true);
```

### ApplePay.MasterCard
Tells Apple Pay to accept or not accept Master Card cards.

OpHps.ApplePay.MasterCard(true | false)

```
OpHps.ApplePay.MasterCard(true);
```

### ApplePay.AmericanExpress
Tells Apple Pay to accept or not accept American Express cards.

OpHps.ApplePay.AmericanExpress(true | false)

```
OpHps.ApplePay.AmericanExpress(true);
```

### ApplePay.Discover
Tells Apple Pay to accept or not accept Discover cards.

OpHps.ApplePay.Discover(true | false)

```
OpHps.ApplePay.Discover(true);
```

### ApplePay.SupportNetwork
Beyond the above supported card networks, you can also add custom card networks as well.

OpHps.ApplePay.SupportNetwork(Card Network, Accept(true | false))

```
OpHps.ApplePay.SupportNetwork('JCB', true)
```

### ApplePay.SetTotal

### ApplePay.ClearLineItems

### ApplePay.AddLineItem

### ApplePay.ClearShippingMethod

### ApplePay.AddShippingMethod

### ApplePay.RequireShippingContactEmail

### ApplePay.RequireShippingContactName

### ApplePay.RequireShippingContactPhone

### ApplePay.RequireShippingContactAddress

### ApplePay.RequiredShippingContactFields

## Testing
