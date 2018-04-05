FROM microsoft/aspnetcore:2 AS importer

ARG ADMIN_API_KEY=athena-sample

RUN apt-get update && \
    DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends postgresql-9.6 && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /athena

# You need to run 'docker build -f src/Athena.Importer/contrib/docker/Dockerfile .' from the solution root
COPY ./_dist/Athena/ ./www
COPY ./_dist/Athena.Importer/ ./importer
COPY ./src/Athena.Importer/data/ ./importer/data
COPY ./src/Athena.Importer/contrib/docker/import.sh ./

ENV ASPNETCORE_URLS=http://0.0.0.0:5000
RUN ./import.sh

FROM postgres:alpine
LABEL maintainer='Athena Developers'
LABEL repo='https://github.com/athena-scheduler/athena'
COPY --from=importer --chown=postgres:postgres /export.sql /docker-entrypoint-initdb.d/export.sql
