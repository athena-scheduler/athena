FROM microsoft/dotnet:2.0-runtime
LABEL maintainer='Athena Developers'
LABEL repo='https://github.com/athena-scheduler/test-data'

WORKDIR /athena

# You need to run 'docker build -f src/Athena.Importer/Dockerfile .' from the solution root
COPY ./_dist/Athena.Importer/ ./
ENTRYPOINT [ "dotnet", "./Athena.Importer.dll" ]