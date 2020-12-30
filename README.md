# OpenPath Heartland Payment Systems Alternative Payment Methods

Alternative Payment Methods (Apple Pay and Google Pay) for Heartland Payment Systems

## Quick Start

To be created!

## OpHps.AltPay.js

### ApiLoginId

This is your unique key that associates your Apple Pay account with OpenPath and Heartland System, which can be found under sites in the OpenPath back office. https://client.openpath.io. For testing purposes, we have provided 10 varified ngrok URLs and matching API Login IDs, which can be found below.

OpHps.ApiLoginId(API Login ID)

```javascript
OpHps.ApiLoginId('XXXXXXXXXXXX');
```

### PaymentCompleteReturnUrl

If the payment is successful this is the URL the server will redirect to with the details of the transaction in the query string.

OpHps.PaymentCompleteReturnUrl(Redirect URL)

```javascript
OpHps.PaymentCompleteReturnUrl('/payment-complete);ja
```

#### Returns the following paramaters

TransactionId
PacketId

```http
/payment-complete?TransactionId=4928399223&PacketId=4949223
```

### EnableLogging

Enable logging turns on the debugging output to the browsers console and to an HTML element if assigned.

OpHps.EnableLogging(true|false)

```javascript
OpHps.EnableLogging(true);
```

### Log

Will output messages to the logging service.

OpHps.Log(Message)

```javascript
OpHps.Log('Hey something happened here!');
```

### AttachLog

If you want the browsers console output from the logger to also be updated on page, which can be useful when testing on mobile devices, you can attach the logger to an HTML element.

OpHps.AttachLog(Element Class or ID)

```javascript
OpHps.AttachLog('#log-output');
```

#### Example HTML Element

```html
<div id="log-output"></div>
```

### ApplePay.AttachApplePayButton

In order to display the Apple Pay button your HTML must have a element that we can attach the button to, use this function to attach that elements Class or ID.

OpHps.ApplePay.AttachApplePayButton(Element Class or ID)

```javascript
OpHps.ApplePay.AttachApplePayButton('.apple-pay-button');
```

#### Example HTML Element

```html
<span class="apple-pay-button hidden"></span>
```

### Attaching Endpoints

Once the Apple Pay button has been displayed and a user clicks on that button, the Apple Pay payment pane will be displayed to the user. In this pane, the user has the ability to change their shipping address, payment types and contact information, as well as choice a shipping method. When these items are changed, the corresponding endpoints will be called below. These endpoints will send the data that was changed and give you the ability to modify the pricing and shipping options available in the pane based on these changes. These endpoints must be available on your server and you must provide the updates.

### ApplePay.AttachPaymentMethodSelectedEndpoint

When a user changes their payment type this attached endpoint will be posted to.

OpHps.ApplePay.AttachPaymentMethodSelectedEndpoint(Endpoint)

```javascript
OpHps.ApplePay.AttachPaymentMethodSelectedEndpoint('/apple-pay/payment-method/' + cartId);
```

### ApplePay.AttachShippingMethodSelectedEndpoint

When a user changes their shipping method this attached endpoint will be posted to.

OpHps.ApplePay.AttachShippingMethodSelectedEndpoint(Endpoint)

```javascript
OpHps.ApplePay.AttachShippingMethodSelectedEndpoint('/apple-pay/shipping-method/' + cartId);
```

### ApplePay.AttachShippingContactSelectedEndpoint

When a user changes their contact information this attached endpoint will be posted to.

OpHps.ApplePay.AttachShippingContactSelectedEndpoint(Endpoint)

```javascript
OpHps.ApplePay.AttachShippingContactSelectedEndpoint('/apple-pay/shipping-contact/' + cartId);
```

### ApplePay.ShowApplePayButton

Once you've completed the initial configuration of Apple Pay, the final is to execute the Show Apple Pay Button function.

OpHps.ApplePay.ShowApplePayButton()

```javascript
OpHps.ApplePay.ShowApplePayButton();
```

### ApplePay.SetCountryCode

To change the default currency of USD, you can set the country code, which in turn will change the currency.

OpHps.ApplePay.SetCountryCode(Two letter country code)

```javascript
OpHps.ApplePay.SetCountryCode('GB');
```

#### Examples

| Country       | Country Code | Currency Code |
| ------------- | ------------ | ------------- |
| United States | US           | USD           |
| Great Britain | GB           | GBP           |

### ApplePay.Visa

Tells Apple Pay to accept or not accept Visa cards.

OpHps.ApplePay.Visa(true | false)

```javascript
OpHps.ApplePay.Visa(true);
```

### ApplePay.MasterCard

Tells Apple Pay to accept or not accept Master Card cards.

OpHps.ApplePay.MasterCard(true | false)

```javascript
OpHps.ApplePay.MasterCard(true);
```

### ApplePay.AmericanExpress

Tells Apple Pay to accept or not accept American Express cards.

OpHps.ApplePay.AmericanExpress(true | false)

```javascript
OpHps.ApplePay.AmericanExpress(true);
```

### ApplePay.Discover

Tells Apple Pay to accept or not accept Discover cards.

OpHps.ApplePay.Discover(true | false)

```javascript
OpHps.ApplePay.Discover(true);
```

### ApplePay.SupportNetwork

Beyond the above supported card networks, you can also add custom card networks as well.

OpHps.ApplePay.SupportNetwork(Card Network, Accept(true | false))

```javascript
OpHps.ApplePay.SupportNetwork('JCB', true)
```

### ApplePay.SetTotal

The set total is the final amount that you want Apple Pay to charge.

OpHps.ApplePay.SetTotal(Total Amount)

```javascript
OpHps.ApplePay.SetTotal(19.99);
```

### Line Items

The below option for line item applies NOT TO PRODUCT LINE ITEMS, but sub-total line items, for example if you wanted to display, tax, shipping and discounts, you would add each of these as a single line item with their amounts.

### ApplePay.ClearLineItems

Clears all the current line items that are or would be displayed in the Apple Pay payment pane.

OpHps.ApplePay.ClearLineItems()

```javascript
OpHps.ApplePay.ClearLineItems();
```

### ApplePay.AddLineItem

Adds a line item to the Apple Pay payment pane.

OpHps.ApplePay.AddLineItem(Label Descriptor, Amount)

```javascript
OpHps.ApplePay.AddLineItem('Sub-total', 19.99);
OpHps.ApplePay.AddLineItem('Discount', -2.00);
OpHps.ApplePay.AddLineItem('Tax', 1.43);
```

### ShippingMethods

Since customer can select their shipping addresses from the Apple Pay payment pane, we need to also give them ability, if you are shipping a product, to select their desired shipping method and shipping prices, the following functions allow you to create available shipping methods and prices.

### ApplePay.ClearShippingMethod

Clears the current shipping methods.

OpHps.ApplePay.ClearShippingMethod()

```javascript
OpHps.ApplePay.ClearShippingMethod();
```

### ApplePay.AddShippingMethod

Adds a shipping options to the Apple Pay payment pane.

OpHps.ApplePay.AddShippingMethod(Name of Shipping Method, Total Shipping Amount, Unique Shipping Code, Additional Details (Shipping Date))

```javascript
OpHps.ApplePay.AddShippingMethod('3 Day Ground', 5.99, '3DG-UPS', 'Arives December 24th, 2021');
```

### ApplePay.RequireShippingContactEmail

Forces the Apple Pay payment panel to require the customers emails address.

OpHps.ApplePay.RequireShippingContactEmail(true | false)

```javascript
OpHps.ApplePay.RequireShippingContactEmail(true);
```

### ApplePay.RequireShippingContactName

Forces the Apple Pay payment panel to require the customers contact name.

OpHps.ApplePay.RequireShippingContactName(true | false)

```javascript
OpHps.ApplePay.RequireShippingContactName(true);
```

### ApplePay.RequireShippingContactPhone

Forces the Apple Pay payment panel to require the customers phone number.

OpHps.ApplePay.RequireShippingContactPhone(true | false)

```javascript
OpHps.ApplePay.RequireShippingContactPhone(true);
```

### ApplePay.RequireShippingContactAddress

Forces the Apple Pay payment panel to require the customers shipping address.

OpHps.ApplePay.RequireShippingContactAddress(true | false)

```javascript
OpHps.ApplePay.RequireShippingContactAddress(true);
```



### ApplePay.RequiredShippingContactFields

Forces the Apple Pay payment panel to require the customers shipping contact fields.

OpHps.ApplePay.RequiredShippingContactFields(true | false)

```javascript
OpHps.ApplePay.RequiredShippingContactFields(true);
```

## Testing

### Test Accounts

| ID   | API Login ID     | Verified Domain                     | ngrok File Name                 |
| ---- | ---------------- | ----------------------------------- | ------------------------------- |
| 0    | gptv7pAxQsyr9UQj | https://op-hps-apple-pay-0.ngrok.io | op-hps-apple-pay-0.ngrok.io.bat |
| 1    | pcjxXKJxZ4pdPASZ | https://op-hps-apple-pay-1.ngrok.io | op-hps-apple-pay-1.ngrok.io.bat |
| 2    | yhvyNkNKkcdWVd9q | https://op-hps-apple-pay-2.ngrok.io | op-hps-apple-pay-2.ngrok.io.bat |
| 3    | 9WPdxye3NRU4zrCv | https://op-hps-apple-pay-3.ngrok.io | op-hps-apple-pay-3.ngrok.io.bat |
| 4    | 47DxjThMjcxtWUsu | https://op-hps-apple-pay-4.ngrok.io | op-hps-apple-pay-4.ngrok.io.bat |
| 5    |                  |                                     |                                 |
| 6    |                  |                                     |                                 |
| 7    |                  |                                     |                                 |
| 8    |                  |                                     |                                 |
| 9    |                  |                                     |                                 |

