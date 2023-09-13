FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RIP_lab01/RIP_lab01.csproj", "RIP_lab01/"]
RUN dotnet restore "RIP_lab01/RIP_lab01.csproj"
COPY . .
WORKDIR "/src/RIP_lab01"
RUN dotnet build "RIP_lab01.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RIP_lab01.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RIP_lab01.dll"]
