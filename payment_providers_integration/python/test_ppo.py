import os

from pago46.ppo import Pago46


os.environ["PROVIDER_KEY"] = "ccc4a3670aca46ff81c9c87dd1d654ce"
os.environ["PAGO46_API_HOST"] = "https://sandboxapi.pago46.io"

pago46 = Pago46()
code = ""
pago46.check(code)