# DummyAppMock

Sample QaaS Mocker project for the code quick start.

## What It Does

- runs the code-defined mock in `DummyAppMock/Program.cs` when no program arguments are passed
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

## Template Check

```bash
dotnet run -- template
```

If you later add a YAML file to the project, pass it explicitly with `dotnet run -- run <file>` instead of relying on a no-args start.
