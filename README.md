# DummyAppMock

Sample QaaS Mocker project for the code quick start.

## What It Does

- keeps `DummyAppMock/Program.cs` as the whole code configuration
- keeps an empty `DummyAppMock/mocker.qaas.yaml` only so `Bootstrap.New(...)` can expose one execution builder
- loads JSON response data from `DummyAppMock/ServerData/sample.json`
- exposes `GET /data` on `http://127.0.0.1:8080`
- uses the same data source, stub, processor configuration, and server shape as the YAML quick start

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
