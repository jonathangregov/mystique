<?php
// ENVIRONMENT VARIABLES FOR CREDENTIALS AND API ENVIRONMENT

$MERCHANT_KEY = getenv("PAGO46_MERCHANT_KEY");
$MERCHANT_SECRET = getenv("PAGO46_MERCHANT_SECRET");
$API_HOST = getenv("PAGO46_API_HOST");

$order_currency = '--local-currency-code--';  // 3 letter currency code (ex: USD, CLP)
$order_id = '--order-id-test--'; // merchant order id for tracking the order
$order_description = '--order-description--'; // merchant order description to display information
$order_price = 1000; // price in integer of the order (applied according to the currency)
$order_timeout = 60; // TTL of the order, this is the time the consumer has to process the order
$merchant_notify_url = 'http://notification.merchant.com'; // merchant notification api
$merchant_return_url = 'http://final.merchant.com'; // return url for the consumer
$request_method = "POST"; // especified in uppercase
$request_path = "/merchant/orders/"; // order creation route

function encodeURIComponent($str) {
    $revert = array('%21'=>'!', '%2A'=>'*', '%27'=>"'", '%28'=>'(', '%29'=>')');
    return strtr(rawurlencode($str), $revert);
}

$date = time();

$params = [
    'currency' => $order_currency,
    'description' => $order_description,
    'merchant_order_id' => $order_id,
    'notify_url' => encodeURIComponent($merchant_notify_url),
    'price' => $order_price,
    'return_url' => encodeURIComponent($merchant_return_url),
    'timeout' => 60
];

$params_string = '';
foreach($params as $key=>$value) { $params_string .= $key.'='.$value.'&'; }

$params_string = rtrim($params_string, '&');

$encrypt_base = $MERCHANT_KEY . '&' . $date .'&' .$request_method . '&' . encodeURIComponent($request_path) . '&'. $params_string;

print $encrypt_base;

$hash = hash_hmac('sha256',$encrypt_base, $MERCHANT_SECRET);

$ch = curl_init();

curl_setopt($ch, CURLOPT_URL, $API_HOST.$request_path);
curl_setopt($ch, CURLOPT_RETURNTRANSFER,true);
curl_setopt($ch, CURLOPT_POSTFIELDS, $params_string);

$headers = [
    'merchant-key: '.$MERCHANT_KEY,
    'message-hash: '.$hash,
    'message-date:'.$date,
];

curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);

$server_output = curl_exec ($ch);

curl_close ($ch);

print $server_output;