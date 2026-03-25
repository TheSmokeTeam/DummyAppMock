# DummyAppMock

Sample QaaS Mocker project for the code quick start.

## What It Does

- runs the code-defined mock in `DummyAppMock/Program.cs` when no program arguments are passed
- runs the checked-in `DummyAppMock/mocker.qaas.yaml` when you pass `run`
- renders the code-defined mock as YAML when you pass `template` without a file path
- loads JSON response data from `DummyAppMock/ServerData/sample.json`
- exposes `GET /data` on `http://127.0.0.1:8080`
- uses the latest public package combination that supports the code-built mock sample

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

## YAML Path

```bash
dotnet run -- run mocker.qaas.yaml
```

## Template Check

```bash
dotnet run -- template
```
