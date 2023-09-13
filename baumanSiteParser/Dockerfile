FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["baumanSiteParser/baumanSiteParser.csproj", "baumanSiteParser/"]
RUN dotnet restore "baumanSiteParser/baumanSiteParser.csproj"
COPY . .
WORKDIR "/src/baumanSiteParser"
RUN dotnet build "baumanSiteParser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "baumanSiteParser.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "baumanSiteParser.dll"]
