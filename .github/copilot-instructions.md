# Copilot instructions — DummyAppMock

Read `AGENTS.md` at the repo root first — it explains the YAML quick-start flow and naming contracts.

Essentials:
- Run: `cd DummyAppMock && dotnet run -- run mocker.qaas.yaml`, then `curl http://127.0.0.1:8080/data`.
- Whole behavior is `mocker.qaas.yaml`: DataSources (FromFileSystem) → Stubs (ServerDataProcessor) → Servers (Http :8080).
- YAML hook names must match discovered class names exactly — renames break silently.
- Pins `QaaS.Mocker` + `QaaS.Common.Generators` versions in `DummyAppMock/DummyAppMock.csproj`; update pinned versions to validate upstream changes.
- Reference consumer: validate Mocker/Framework changes against this repo end-to-end.
