import requests

from pago46.utils import sign_request


class Pago46(object):
    """Pago46 Client"""
    url_merchant_order = "/merchant/order"
    url_merchant_orders = "/merchant/orders"
    url_merchant_complete = "/merchant/complete/"
    url_merchant_notification = "/merchant/notification"

    def __init__(self, merchant_key=None, merchant_secret=None, pago46_api_host=None):
        """PARAMS to inicialize the client of PAGO46"""
        self.merchant_key = self.__get_merchant_key(merchant_key)
        self.merchant_secret = self.__get_merchant_secret(merchant_secret)
        self.pago46_api_host = self.__get_pago46_api_host(pago46_api_host)

    def __get_merchant_key(self, merchant_key):
        if not merchant_key:
            raise TypeError("needs a merchant_key")
        return merchant_key

    def __get_merchant_secret(self, merchant_secret):
        if not merchant_secret:
            raise TypeError("needs a merchant_secret")
        return merchant_secret

    def __get_pago46_api_host(self, pago46_api_host):
        if not pago46_api_host:
            raise TypeError("needs a pago46_api_host")
        return pago46_api_host

    def get_all_orders(self):
        """ Get all orders from a e-merchant"""
        method = "GET"
        message_hash, date = sign_request(method=method, path=self.url_merchant_orders, merchant_key=self.merchant_key,
                                          merchant_secret=self.merchant_secret)
        headers = {
            "merchant-key": self.merchant_key,
            "message-hash": message_hash,
            "message-date": date
        }
        url = "{}{}".format(self.pago46_api_host, self.url_merchant_orders)
        response = requests.get(url, headers=headers)
        return response

    def create_order(self, payload):
        """Create a order on Pago46
            params:
             -merchant_order_id
             -currency
             -price
             -timeout
             -notify_url
             -return_url
             -description (opcional)
        """
        method = "POST"
        message_hash, date = sign_request(method=method, path=self.url_merchant_orders, payload=payload,
                                          merchant_key=self.merchant_key, merchant_secret=self.merchant_secret)
        headers = {
            "merchant-key": self.merchant_key,
            "message-hash": message_hash,
            "message-date": date
        }
        url = "{}{}".format(self.pago46_api_host, self.url_merchant_orders)
        response = requests.post(url, data=payload, headers=headers)
        return response

    def mark_order_as_complete(self, payload):
        method = "POST"
        message_hash, date = sign_request(method=method, path=self.url_merchant_complete, payload=payload,
                                          merchant_key=self.merchant_key, merchant_secret=self.merchant_secret)
        headers = {
            "merchant-key": self.merchant_key,
            "message-hash": message_hash,
            "message-date": date
        }
        url = "{}{}".format(self.pago46_api_host, self.url_merchant_complete)
        response = requests.post(url, data=payload, headers=headers)
        return response

    def get_order_by_id(self, order_id):
        method = "GET"
        path = "{}/{}".format( self.url_merchant_order, order_id)
        message_hash, date = sign_request(method=method, path=path, merchant_key=self.merchant_key,
                                          merchant_secret=self.merchant_secret)
        headers = {
            "merchant-key": self.merchant_key,
            "message-hash": message_hash,
            "message-date": date
        }
        url = "{}{}/{}".format(self.pago46_api_host, self.url_merchant_order, order_id)

        response = requests.get(url, headers=headers)
        return response

    def get_order_by_notification_id(self, notification_id):
        method = "GET"
        path = "{}/{}".format(self.url_merchant_notification, notification_id)
        message_hash, date = sign_request(method=method, path=path, merchant_key=self.merchant_key,
                                          merchant_secret=self.merchant_secret)
        headers = {
            "merchant-key": self.merchant_key,
            "message-hash": message_hash,
            "message-date": date
        }

        url = "{}{}/{}".format(self.pago46_api_host, self.url_merchant_notification, notification_id)

        response = requests.get(url, headers=headers)
        return response

    def get_order_details_by_order_id(self, order_id):
        method = "GET"
        path = "{}/{}/detail".format(self.url_merchant_order, order_id)
        message_hash, date = sign_request(method=method, path=path, merchant_key=self.merchant_key,
                                          merchant_secret=self.merchant_secret)
        headers = {
            "merchant-key": self.merchant_key,
            "message-hash": message_hash,
            "message-date": date
        }
        url = "{}{}/{}/detail".format(self.pago46_api_host, self.url_merchant_order, order_id)
        response = requests.get(url, headers=headers)
        return response