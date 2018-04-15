# Athena Scheduling Assistant

| Windows | Linux | Style | Coverage |
| ------- | ----- | ----- | -------- |
| [![Build status](https://ci.appveyor.com/api/projects/status/sfdfysdjn9806apq/branch/master?svg=true)](https://ci.appveyor.com/project/athena-scheduler/athena/branch/master) | [![Build Status](https://travis-ci.org/athena-scheduler/athena.svg?branch=master)](https://travis-ci.org/athena-scheduler/athena) | [![Codacy Badge](https://api.codacy.com/project/badge/Grade/013e28793a554168b6f2ac337df77ebc)](https://www.codacy.com/app/athena-scheduler/athena?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=athena-scheduler/athena&amp;utm_campaign=Badge_Grade) | [![codecov](https://codecov.io/gh/athena-scheduler/athena/branch/master/graph/badge.svg)](https://codecov.io/gh/athena-scheduler/athena) |

A tool for assisting students with scheduling for courses.

## Usage

The easiest way to run the scheduler is via docker. This repository contains an example `docker-compose`
file which is based off of [sample data](src/Athena.Importer/data/). If you want to enable authentication
from external login providers (which you will need to do in order to do anything useful), copy `auth-keys.env.sample`
to `auth-keys.env` and supply your google or microsoft OAuth2 keys:

```bash
cp auth-keys.env.sample auth-keys.env
# Edit auth-keys.env to use your keys

docker-compose pull
docker-compose up
```

The web interface is now accessible at http://localhost:5000/. The API is on `/api` and the default admin
api key for this sample data is `athena-sample`. Feel free to import more data.

If you want to start from scratch, stand up a postgres 9.6 database and change the connection string in
`docker-compose.yml` appropriately.

## Building

Invoke the build build script:

```bash
./build.sh --UseDocker=true
```

Or on windows:

```powershell
./build.ps1 --UseDocker=true
```

This will start a docker container to run database tests. If you already have a postgres instance, set the
`ATHENA_DATA_TESTS_CON` environment variable to the connection string before building.

### IDE's

If you are building / debugging in an IDE, you will need a postgres database. We recommend using docker:

```bash
docker run -it --rm -p 5432:5432 postgres:alpine
```

If you have another database you wish to connect to, set the `ATHENA_CONNECTION_STRING` environment variable.

You will need to install client-side packages and run the bundler manually. Client packages can be installed
via npm:

```bash
cd src/Athena
npm install
```

To run the bundler in the background, run the `bundle::watch` script, which will repackage stylesheets and
scripts when you save them in your IDE:

```bash
cd src/Athena
npm run bundle::watch
```

## Authentication

Athena uses third party OAuth2 providers for authentication Currently, the following providers are setup:

* Google
  * Follow the [ASP.NET Core Guide for Configuring Google Auth](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?tabs=aspnetcore2x#create-the-app-in-google-api-console) to create the client secret and client id. Set the `AUTH_GOOGLE_CLIENT_KEY` and `AUTH_GOOGLE_CLIENT_SECRET` environment variables to enable google authentication.
* Microsoft
  * Follow the [ASP.NET Core Guide for Configuring Microsoft Auth](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins?tabs=aspnetcore2x) to create the client secret and client id. Set the `AUTH_MICROSOFT_CLIENT_KEY` and `AUTH_MICROSOFT_CLIENT_SECRET` environment variables to enable microsoft authentication.

## License

Athena is licensed under the `MIT` license. See [`LICENSE.md`](/LICENSE.md) for details.
