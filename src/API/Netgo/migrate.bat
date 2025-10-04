#!/bin/bash
set -e 
docker build -f Netgo.Migrator.Dockerfile -t netgo-migrator . 
docker run --rm --network dockercompose15106419983109714426_default netgo-migrator