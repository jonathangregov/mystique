import json
import os
import six

from unittest import TestCase
from unittest import mock

from pago46.providers import Pago46


class Pago46PaymentProviderTestCase(TestCase):
    def setUp(self):
        """
        Set all enviroment variables to init Pago46.
        """
        os.environ["PROVIDER_KEY"] = "secret_provider_key"
        os.environ["PAGO46_API_HOST"] = "https://sandboxapi.pago46.io"

    def test_called_with_all_arguments(self):
        """
        init Pago46 can be done with all it's arguments
        """
        client = Pago46()
        self.assertEqual(client.provider_key, "secret_provider_key")
        self.assertEqual(client.pago46_api_host, "https://sandboxapi.pago46.io")

    def test_no_provider_key_given(self):
        """
        init Pago46 raises an exception if no provider_key is given
        """
        del os.environ["PROVIDER_KEY"]

        six.assertRaisesRegex(
            self,
            KeyError,
            "needs a environment variable called PROVIDER_KEY",
            Pago46,
        )

    def test_no_pago46_api_host_given(self):
        """
        init Pago46 raises an exception if no pago46_api_host is given
        """
        del os.environ["PAGO46_API_HOST"]

        six.assertRaisesRegex(
            self,
            KeyError,
            "needs a environment variable called PAGO46_API_HOST",
            Pago46,
        )

    # @mock.patch("pago46.client.requests.get")
    def test_provider_check_successful(self):
        """
        Create a new order on Pago46 Payment providers orders system.
        """
        client = Pago46()

        response = {
            "code": "",
            "price": "",
            "price_currency": "",
            "status": "",
            "creation_date": "",
            "last_notify_date": "",
        }

        # mock_post.return_value = mock.MagicMock(
        #     headers={"content-type": "application/json"},
        #     status_code=200,
        #     response=json.dumps(response),
        # )

        code = "123456789"

        response = client.check(code)
        self.assertEqual(response.status_code, 200)

    # @mock.patch("pago46.client.requests.post")
    def test_provider_notify_sucessful(self):
        client = Pago46()
        response = {
            "price": "50",
            "price_currency": "USD",
            "code": "2319359630",
            "status": "complete",
            "creation_date": "2021-02-18T19:22:46.701062Z",
            "last_notify_date": "2021-02-23T17:19:41.047885Z",
        }
        # mock_get.return_value = mock.MagicMock(
        #     headers={"content-type": "application/json"},
        #     status_code=200,
        #     response=json.dumps(response),
        # )

        payload = {"status": "complete"}
        response = client.notify(order_id)

        self.assertEqual(response.status_code, 200)
