# DummyAppMock

Sample QaaS Mocker project for the code quick start.

## What It Does

- builds the mock runtime directly in `DummyAppMock/Program.cs`
- loads JSON response data from `DummyAppMock/ServerData/sample.json`
- exposes `GET /data` on `http://127.0.0.1:8080`
- uses the latest public `QaaS.Mocker` / `QaaS.Common.Generators` packages from `nuget.org`

## Run

```bash
dotnet restore
dotnet run --project DummyAppMock/DummyAppMock.csproj
```

Then call:

```bash
curl http://127.0.0.1:8080/data
```
