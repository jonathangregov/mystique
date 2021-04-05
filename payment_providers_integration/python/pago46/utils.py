import hashlib
import hmac
import time
import urllib.parse
from collections import OrderedDict


def sign_request(method, path, merchant_key, payload={}):
    """sign a request to CPP of PAGO46"""
    date = str(time.time()).replace(".", "")
    encrypt_base = urllib.parse.urlencode(params)

    message_hash = hmac.new(
        merchant_secret.encode("utf-8"), encrypt_base.encode("utf-8"), hashlib.sha256
    ).hexdigest()
    return message_hash, date
