# AGENTS.md — DummyAppMock

Guidance for AI agents working in this repository.

## What this repo is

The **reference QaaS.Mocker testbed**: a minimal mock HTTP server proving the YAML quick-start path. It loads JSON from `ServerData/sample.json` via the `FromFileSystem` generator and serves it on `GET http://127.0.0.1:8080/data` through a custom `ServerDataProcessor`. This repo is the canary for Mocker changes — Framework/Mocker/Common.* changes should be validated against it end-to-end.

## Layout

| Path | Purpose |
|---|---|
| `DummyAppMock/Program.cs` | bootstrap: `QaaS.Mocker.Bootstrap.New(args).Run()` |
| `DummyAppMock/mocker.qaas.yaml` | the whole config: DataSources → Stubs → Servers(Http :8080, `/data`) |
| `DummyAppMock/ServerData/sample.json` | served payload |
| `DummyAppMock/Processors/` | custom `ServerDataProcessor` (status 200, JSON) |
| `NuGet.config` | feed configuration |
| `Dockerfile` | containerized mock server |

## Run

```powershell
dotnet restore
dotnet run --project DummyAppMock -- run mocker.qaas.yaml
# then: curl http://127.0.0.1:8080/data
```

## Critical gotchas

- Consumes **latest public** `QaaS.Mocker` + `QaaS.Common.Generators` from nuget.org — a breaking upstream release shows up here first. Pin versions when reproducing issues.
- YAML names are contracts: `Generator: FromFileSystem`, `Processor: ServerDataProcessor`, `TransactionStubName: ServerDataStub` must match discovered hook class names exactly.
- Local checkouts may sit on the `yaml_configuration` branch — check the branch before comparing behavior with remote default.
- Custom processors here are discovered via Framework assembly scanning (user-assembly tier) — namespace/class renames break YAML references silently.

## Process

This is a testbed: keep it tiny and demonstrative. Validate any QaaS package bump by running the server and curling `/data`. Conventional commits.
