import os

import requests

from pago46.utils import sign_request


class Pago46:
    """Pago46 Client"""

    path_provider_check = "/provider/check"
    path_provider_notify = "/provider/notify"

    def __init__(self):
        """PARAMS to inicialize the client of PAGO46"""
        self.merchant_key = self.__get_merchant_key()
        self.merchant_secret = self.__get_merchant_secret()
        self.pago46_api_host = self.__get_pago46_api_host()

    def __get_merchant_key(self):
        if not "PROVIDER_KEY" in os.environ:
            raise KeyError("needs a environment variable called PROVIDER_KEY")
        return os.environ["PROVIDER_KEY"]

    def __get_pago46_api_host(self):
        if not "PAGO46_API_HOST" in os.environ:
            raise KeyError("needs a environment variable called PAGO46_API_HOST")
        return os.environ["PAGO46_API_HOST"]

    def check(self, code: str):
        """ Get all orders from a e-merchant"""
        method = "GET"
        message_hash, date = sign_request(
            method=method,
            path=self.url_merchant_orders,
            merchant_key=self.merchant_key,
            merchant_secret=self.merchant_secret,
        )
        headers = {
            "merchant-key": self.merchant_key,
            "message-hash": message_hash,
            "message-date": date,
        }
        url = "{}{}".format(self.pago46_api_host, self.url_merchant_orders)
        response = requests.get(url, headers=headers, verify=False)
        return response

    def notify(self, code: str):
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
        message_hash, date = sign_request(
            method=method,
            path=self.url_merchant_orders,
            payload=payload,
            merchant_key=self.merchant_key,
            merchant_secret=self.merchant_secret,
        )
        headers = {
            "merchant-key": self.merchant_key,
            "message-hash": message_hash,
            "message-date": date,
        }
        url = "{}{}".format(self.pago46_api_host, self.url_merchant_orders)
        response = requests.post(url, data=payload, headers=headers, verify=False)
        return response
