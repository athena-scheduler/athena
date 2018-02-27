FROM microsoft/aspnetcore:2
LABEL maintainer="Athena Developers"

ENV ASPNETCORE_URLS=http://0.0.0.0:5000
EXPOSE 5000

COPY _dist/Athena/ /
CMD ["dotnet", "/Athena.dll"]
