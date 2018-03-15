#!/bin/bash

set -euo pipefail

PG_HOST="${1}"


until psql -h "${PG_HOST}" -U "postgres" -c '\q' >/dev/null 2>&1; do
    >&2 echo "Waiting for postgres to start"
    sleep 1
done

>&2 echo "Postgres started"
dotnet /Athena.dll