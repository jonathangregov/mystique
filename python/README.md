# Pago46 Package

Libreria Python para la integración con la Plataforma de pago en efectivo de Pago46.

## Descripción

Esta librería esta pensada para poder entregar a los e-merchant de pago46 una forma estandar, facil y rapida,
de integrar sus productos/servicios con pago 46 para poder ofrecer la opción de pago en efectivo a sus clientes.


### Configuración.

Se debe tener un merchant_secret, merchant_key para poder generar llamadas a la plataforma de pago46

```python
from Pago46.client import Pago46

pago46_api_host = "https://sandboxapi.pago46.com"  # for testing  or "https://api.pago46.com" for production
merchant_secret = "<merchant_secret>" # merchat_secret otorgda por pago46
merchant_key = "<merchant_key>" # llave secreta otorgada por pago46


```
Para inicializar el cliente se deben pasar como argumento merchant_secret, merchant_secret y la url a la cual se esta apuntando
el cual puede ser producción o servidor de pruebas para testing.

```python
from Pago46.client import Pago46

pago46_api_host = "https://sandboxapi.pago46.com"  # for testing  or "https://api.pago46.com" for production
merchant_secret = "<merchant_secret>"
merchant_key = "<merchant_key>"

client = Pago46(merchant_key, merchant_secret, pago46_api_host)

```
Ejemplo creación de orden.

```python
from Pago46.client import Pago46

pago46_api_host = "https://sandboxapi.pago46.com"  # for testing  or "https://api.pago46.com" for production
merchant_secret = "<merchant_secret>"
merchant_key = "<merchant_key>"

client = Pago46(merchant_key, merchant_secret, pago46_api_host)

payload = {
    "currency": "CLP", # Tipo de moneda 
    "merchant_order_id": '0001', # id que identifica una transacción.
    "notify_url":"http://merchant.com/app/response", # La URL en la que pago46 publicara la respuesta al modificarse el estado de la transacción.
    "price": 1000,# precio de la orden
    "return_url": "http://final.merchant.com",# url a la cual el user será redirigido al terminar el proceso.
    "timeout": 60, # duración en que la transacción estará activa para ser pagada en minutos.
    "description": "description of product.", # (opcional): descripción opcional del producto/servicio.

}
# create a new order

response = client.create_order(payload)
```

Ejemplo marcar una orden como completa.

```
payload = {"order_id": "0001"}
response = client.mark_order_as_complete(payload)
```

Ejemplo de obtener una orden por su ID

```
order_id = "0001"
response = client.get_order_by_id(order_id)
```

Ejemplo de obtener una order por su NOTIFICATION ID

```
notification_id = "fe0eac28aa774b539b0e12d0227bf27f"
response=  client.get_order_by_notification_id(notification_id)
```
Ejemplo de obtener los detalles de una orden por su ORDER ID


```
order_id = "121d3b2c-b985-4592-b8fc-b5c6d9ce5a13"
response = client.get_order_details_by_order_id(order_id)
```


### Instalación

Instalar libreria a traves PIP 

```
pip install Pago46
```

