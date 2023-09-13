FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BmstuSiteParser/BmstuSiteParser.csproj", "BmstuSiteParser/"]
RUN dotnet restore "BmstuSiteParser/BmstuSiteParser.csproj"
COPY . .
WORKDIR "/src/BmstuSiteParser"
RUN dotnet build "BmstuSiteParser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BmstuSiteParser.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BmstuSiteParser.dll"]
