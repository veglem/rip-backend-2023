FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebApi.Host/WebApi.Host.csproj", "WebApi.Host/"]
RUN dotnet restore "WebApi.Host/WebApi.Host.csproj"
COPY . .
WORKDIR "/src/WebApi.Host"
RUN dotnet build "WebApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.Host.dll"]
