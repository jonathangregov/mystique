# Pago46 Javascript Package

Javascript Library for the integration with the cash payment plataform of pago46.

## Description

This library it was developed for give e-merchants of pago46 a standard, easy and fast integration to integrate his products/services with Pago46 to can offer the option of pay with cash to his clients


###  Installation

You can install Pago46 Package in the usual ways. The simplest way is with npm:

```
npm i --save pago46
```

### Configuration.

to configure the client of Pago46 it's necessary to have a MERCHANT_SECRET and MERCHANT_KEY (those key are provided by Pago46)
with those keys we can generate calls to PAGO46 API.

we must configure the MERCHANT_SECRET, MERCHANT_KEY and PAGO46_API_HOST on enviroment variables.


Example
```javascript
process.env.PAGO46_MERCHANT_KEY = '<secret>'
process.env.PAGO46_MERCHANT_SECRET = '<secret>'
process.env.PAGO46_API_HOST =  "http://sandboxapi.pago46.com" # for testing  or "https://api.pago46.com" for production
```

with the environment variables set, we can intilialize the client 


```javascript
var client = require('pago46')
```
Example create a order

```javascript
var payload = {
     'currency': 'CLP',
     'description': 'description testing from Javscript library V1',
     'merchant_order_id': 'testJS1',
     'notify_url': 'http://merchant.com/app/response',
     'price': 1000,
     'return_url': 'http://final.merchant.com',
     'timeout': 600
}
client.createOrder(payload)
```

Example to mark a order as complete.

```javascript
var payload = {"order_id": "7b41ae99-ebdb-4fbc-a1e7-0922d84496f0"}
client.markOrderAsComplete(payload)
```
Example get a order by ID

```javascript
var order_id = "7b41ae99-ebdb-4fbc-a1e7-0922d84496f0"
client.getOrderById(order_id)
```
Example get a order by Notification ID

```javascript
var notification_id = "fe0eac28aa774b539b0e12d0227bf27f"
client.getOrderByNotificationId(notification_id)
```
Example get order details by order ID

```javascript
var order_id = "7b41ae99-ebdb-4fbc-a1e7-0922d84496f0"
client.getOrderDetailsByOrderId(order_id)
```

