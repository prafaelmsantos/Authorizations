#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release

#Add GitHub NuGet sources
ARG GITHUB_PASSWORD

WORKDIR /src
COPY ["src/API/Authorizations.API/Authorizations.API.csproj", "src/API/Authorizations.API/"]
COPY ["src/Infrastructure/Authorizations.GraphQL/Authorizations.GraphQL.csproj", "src/Infrastructure/Authorizations.GraphQL/"]
COPY ["src/Core/Authorizations.Core/Authorizations.Core.csproj", "src/Core/Authorizations.Core/"]
COPY ["src/Infrastructure/Authorizations.Persistence/Authorizations.Persistence.csproj", "src/Infrastructure/Authorizations.Persistence/"]
RUN dotnet nuget add source -n github -u AZ -p ${GITHUB_PASSWORD} --store-password-in-clear-text https://nuget.pkg.github.com/prafaelmsantos/index.json
RUN dotnet restore "./src/API/Authorizations.API/Authorizations.API.csproj"
COPY . .
WORKDIR "/src/src/API/Authorizations.API"
RUN dotnet build "./Authorizations.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Authorizations.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authorizations.API.dll"]