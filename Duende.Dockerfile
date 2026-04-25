FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["DuendeIdentityServer/DuendeIdentityServer.csproj", "/src/DuendeIdentityServer/"]
COPY ["Webshop.Domain/Webshop.Domain.csproj", "Webshop.Domain/"]
COPY ["Webshop.Infrastructure/Webshop.Infrastructure.csproj", "Webshop.Infrastructure/"]
COPY ["Webshop.Application/Webshop.Application.csproj", "Webshop.Application/"]
RUN dotnet restore "DuendeIdentityServer/DuendeIdentityServer.csproj"
COPY . .
WORKDIR "/src/DuendeIdentityServer"
RUN dotnet build "/src/DuendeIdentityServer/DuendeIdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/DuendeIdentityServer/DuendeIdentityServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DuendeIdentityServer.dll"]
