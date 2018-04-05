#!/usr/bin/env bash
set -euo pipefail

ADMIN_API_KEY="${ADMIN_API_KEY:-athena-sample}"

# Setup Database
su -c '/usr/lib/postgresql/9.6/bin/initdb --username=postgres /var/lib/postgresql/data' postgres
sed -i 's|^data_directory = .*$|data_directory = '\''/var/lib/postgresql/data'\''|g' /etc/postgresql/9.6/main/postgresql.conf
sed -i -e 's/peer/trust/g' -e 's/md5/trust/g' /etc/postgresql/9.6/main/pg_hba.conf
service postgresql start

# Migrate and create admin account
pushd /athena/www
ATHENA_INIT_ONLY=1 dotnet ./Athena.dll

# Reset admin api key
psql -U postgres -c "UPDATE users SET api_key='${ADMIN_API_KEY}'"

# Run API for importer
dotnet ./Athena.dll &
ATHENA_API_PID=$!
popd

# Run Importer
pushd /athena/importer
sleep 10
dotnet ./Athena.Importer.dll --api-endpoint http://localhost:5000/api --data-path ./data --api-key "${ADMIN_API_KEY}"
popd

# Make an archive
pg_dumpall -U postgres > /export.sql
sed -i -e 's/CREATE ROLE postgres;//g' /export.sql

# Cleanup
kill $ATHENA_API_PID
service postgresql stop