"""is the build script for setuptools. It tells setuptools about your """

import setuptools

with open("README.md", "r") as fh:
    LONG_DESCRIPTION = fh.read()

setuptools.setup(
    name="pago46",
    version="0.0.1",
    author="David Liencura",
    author_email="dliencura@46degrees.net",
    description="INtegraton package of pago46",
    long_description=LONG_DESCRIPTION,
    long_description_content_type="text/markdown",
    url="",
    packages=setuptools.find_packages(),
    classifiers=(
        "Programming Language :: Python :: 3",
        "License :: OSI Approved :: MIT License",
        "Operating System :: OS Independent",
    ),
)
