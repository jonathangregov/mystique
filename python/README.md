# Pago46 Package

Libreria Python para la integración con la Plataforma de pago en efectivo de Pago46.

## Descripción

Esta librería esta pensada para poder entregar a los e-merchant de pago46 una forma estandar, facil y rapida,
de integrar sus productos/servicios con pago 46 para poder ofrecer la opción de pago en efectivo a sus clientes.


### Configuración.

Se debe tener un merchant_secret y merchant_key para poder generar llamadas a la plataforma de pago46

```python
from pago46_pkg.pago46 import Pago46

pago46_api_host = "https://sandboxapi.pago46.com"  # sandbox or live
merchant_secret = "<merchant_secret>"
merchant_key = "<merchant_key>"


```
Para inicializar el cliente se deben pasar como argumento merchant_secret, merchant_secret y la url a la cual se esta apuntando
el cual puede ser producción o servidor de pruebas para testing.

```python
from pago46_pkg.pago46 import Pago46

pago46_api_host = "https://sandboxapi.pago46.com"  # sandbox or live
merchant_secret = "<merchant_secret>"
merchant_key = "<merchant_key>"

client = Pago46(merchant_key, merchant_secret, pago46_api_host)

```
Ejemplo creación de orden.

```python
from pago46_pkg.pago46 import Pago46

pago46_api_host = "https://sandboxapi.pago46.com"  # sandbox or live
merchant_secret = "<merchant_secret>"
merchant_key = "<merchant_key>"

client = Pago46(merchant_key, merchant_secret, pago46_api_host)

payload = {
    "currency": "CLP",
    "merchant_order_id": 'testpythonlibrary1',
    "notify_url":"http://merchant.com/app/response",
    "price": 1000,
    "return_url": "http://final.merchant.com",
    "timeout": 60,
    "description": "description of product.",

}
# create a new order

response = client.create_order(payload)
```

### Instalación

Instalar libreria a traves PIP 

```
pip install Pago46
```


## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details


