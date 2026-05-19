# DummyAppMock

Sample QaaS Mocker project for the code quick start.

## What It Does

- keeps `DummyAppMock/Program.cs` as the whole code configuration
- loads JSON response data from `DummyAppMock/ServerData/sample.json`
- exposes `GET /data` on `http://127.0.0.1:8080`
- uses the same data source, stub, and server shape as the YAML quick start

## Run

```bash
export QAAS_NUGET_SOURCE_URL=https://api.nuget.org/v3/index.json
dotnet restore --source "$QAAS_NUGET_SOURCE_URL"
cd DummyAppMock
dotnet run
```

Then call:

```bash
curl http://127.0.0.1:8080/data
```
