FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Webshop.MVC/Webshop.MVC.csproj", "Webshop.MVC/"]
COPY ["Webshop.Domain/Webshop.Domain.csproj", "Webshop.Domain/"]
COPY ["Webshop.Infrastructure/Webshop.Infrastructure.csproj", "Webshop.Infrastructure/"]
#install packages
RUN dotnet restore "./Webshop.MVC/Webshop.MVC.csproj"
COPY . .
WORKDIR "/src/Webshop.MVC"
RUN dotnet build "./Webshop.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Webshop.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webshop.MVC.dll"]