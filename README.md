# DummyAppMock

Sample QaaS Mocker project for the YAML quick start.

## What It Does

- loads JSON response data from `DummyAppMock/ServerData/sample.json`
- exposes `GET /data` on `http://127.0.0.1:8080`
- uses a local `ServerDataProcessor` and the latest public `QaaS.Mocker` / `QaaS.Common.Generators` packages from `nuget.org`

## Run

```bash
dotnet restore
cd DummyAppMock
dotnet run
```

Then call:

```bash
curl http://127.0.0.1:8080/data
```
