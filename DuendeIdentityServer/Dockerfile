FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["DuendeIdentityServer/DuendeIdentityServer.csproj", "src/DuendeIdentityServer/"]
RUN dotnet restore "src/DuendeIdentityServer/DuendeIdentityServer.csproj"
COPY . .
WORKDIR "/src/DuendeIdentityServer"
RUN dotnet build "DuendeIdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DuendeIdentityServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DuendeIdentityServer.dll"]
