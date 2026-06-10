# Copilot instructions — DummyAppMock

Read `AGENTS.md` at the repo root first — it explains the YAML quick-start flow and naming contracts.

Essentials:
- Run: `dotnet run --project DummyAppMock -- run mocker.qaas.yaml`, then `curl http://127.0.0.1:8080/data`.
- Whole behavior is `mocker.qaas.yaml`: DataSources (FromFileSystem) → Stubs (ServerDataProcessor) → Servers (Http :8080).
- YAML hook names must match discovered class names exactly — renames break silently.
- Consumes latest public QaaS.Mocker/Common.Generators from nuget.org; pin versions to reproduce issues.
- Reference consumer: validate Mocker/Framework changes against this repo end-to-end.
