ARG DOTNET_SDK_IMAGE=mcr.microsoft.com/dotnet/sdk:10.0
ARG DOTNET_ASPNET_IMAGE=mcr.microsoft.com/dotnet/aspnet:10.0

FROM ${DOTNET_SDK_IMAGE} AS build
WORKDIR /src
COPY . .
ARG QAAS_NUGET_SOURCE_URL=https://api.nuget.org/v3/index.json
ENV QAAS_NUGET_SOURCE_URL=${QAAS_NUGET_SOURCE_URL}
RUN dotnet restore DummyAppMock.sln --configfile NuGet.config --source "${QAAS_NUGET_SOURCE_URL}"
RUN dotnet publish DummyAppMock/DummyAppMock.csproj -c Release -o /app/publish --no-restore

FROM ${DOTNET_ASPNET_IMAGE}
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DummyAppMock.dll"]
