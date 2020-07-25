FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
ARG config

#copy all folders
WORKDIR /src
COPY ["WebServer/WebServer.csproj", "WebServer/"]
COPY ["WebServer.BLL.Application/WebServer.BLL.Application.csproj", "WebServer.BLL.Application/"]
COPY ["WebServer.BLL.Domain/WebServer.BLL.Domain.csproj", "WebServer.BLL.Domain/"]
COPY ["WebServer.DAL/WebServer.DAL.csproj", "WebServer.DAL/"]

COPY . .

#publish
FROM build AS publish
ARG config

RUN dotnet publish WebServer/WebServer.csproj -c $config -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebServer.dll"]