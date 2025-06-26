FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder

WORKDIR /src

COPY ["NexFB.WCFService/NexFB.WCFService.csproj", "NexFB.WCFService/"]

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore -v n "NexFB.WCFService/NexFB.WCFService.csproj"

COPY ["NexFB.WCFService/", "NexFB.WCFService/"]
COPY ["Dependencies/AutoDetectLanguage/", "Dependencies/AutoDetectLanguage/"]
WORKDIR /src/NexFB.WCFService

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -r linux-x64 --self-contained false -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-alpine AS base

# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

WORKDIR /app
COPY --from=builder /app .
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "NexFB.WCFService.dll"]