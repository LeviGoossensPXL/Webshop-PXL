FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebApi.webapi/WebApi.webapi.csproj", "WebApi.webapi/"]
COPY ["Webshop.Domain/Webshop.Domain.csproj", "Webshop.Domain/"]
#install packages
RUN dotnet restore "./WebApi.webapi/WebApi.webapi.csproj"
COPY . .
WORKDIR "/src/WebApi.webapi"
RUN dotnet build "./WebApi.webapi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.webapi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.webapi.dll"]