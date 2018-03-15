FROM microsoft/aspnetcore:2
LABEL maintainer="Athena Developers"

ENV ASPNETCORE_URLS=http://0.0.0.0:5000
EXPOSE 5000

RUN DEBIAN_FRONTEND=noninteractive apt-get update && \
    apt-get install -y --no-install-recommends postgresql-client && \
    rm -rf /var/lib/apt/lists/*

COPY contrib/ /contrib
COPY _dist/Athena/ /
CMD ["dotnet", "/Athena.dll"]
