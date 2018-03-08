FROM microsoft/dotnet:2.0-runtime
LABEL maintainer='Athena Developers'
LABEL repo='https://github.com/athena-scheduler/test-data'

WORKDIR /athena
COPY ./_dist/Athena.Importer/ ./
ENTRYPOINT [ "dotnet", "./Athena.Importer.dll" ]