#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebHostIU/WebHostIU.csproj", "WebHostIU/"]
RUN dotnet nuget add source "https://gitlab.com/api/v4/groups/13891093/-/packages/nuget/index.json"  \
-n gitlab \
-u "gitlab+deploy-token-671968" \
-p k_hTupnBgXE2TySqX25s\
 --store-password-in-clear-text && dotnet nuget enable source gitlab
RUN dotnet restore "WebHostIU/WebHostIU.csproj" 
COPY . .
WORKDIR "/src/WebHostIU"
RUN dotnet build "WebHostIU.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebHostIU.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebHostIU.dll"]
