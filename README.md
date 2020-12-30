# OpenPath Heartland Payment Systems Alternative Payment Methods
Alternative Payment Methods (Apple Pay and Google Pay) for Heartland Payment Systems

## Quick Start
To be created!

## OpHps.AltPay.js
### ApiLoginId
This is your unique key that associates your Apple Pay account with OpenPath and Heartland System, which can be found under sites in the OpenPath backoffice. https://client.openpath.io. For testing purposes, we have provided 10 varified ngrok URLs and matching API Login IDs, which can be found below.

OpHps.ApiLoginId(API Login ID)
`OpHps.ApiLoginId('XXXXXXXXXXXX');`

### PaymentCompleteReturnUrl
If the payment is successful this is the URL the server will redirect to with the details of the transaction in the query string.

OpHps.PaymentCompleteReturnUrl(Redirect URL)
`OpHps.PaymentCompleteReturnUrl('/payment-complete);`

#### Returns the following paramaters
TransactionId
PacketId
`/payment-complete?TransactionId=4928399223&PacketId=4949223`

### EnableLogging
Enable logging turns on the debugging output to the browsers console and to an HTML element if assigned.

OpHps.EnableLogging(true|false)
`OpHps.EnableLogging(true);`

### Log
Will output messages to the logging service.

OpHps.Log(Message)
`OpHps.Log('Hey something happened here!');`

### ApplePay.AttachApplePayButton
In order to display the Apple Pay button your HTML must have a element that we can attach the button to, use this function to attach that elements Class or ID.

OpHps.ApplePay.AttachApplePayButton(Element Class or ID)
`OpHps.ApplePay.AttachApplePayButton('.apple-pay-button');`

#### Example HTML Element
```
<span class="apple-pay-button apple-pay-button-white hidden"></span>
```

### ApplePay.AttachLog

### ApplePay.AttachPaymentMethodSelectedEndpoint

### ApplePay.AttachShippingMethodSelectedEndpoint

### ApplePay.AttachShippingContactSelectedEndpoint

### ApplePay.ShowApplePayButton

### ApplePay.SetCountryCode

### ApplePay.Visa

### ApplePay.MasterCard

### ApplePay.AmericanExpress

### ApplePay.Discover

### ApplePay.SupportNetwork

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
