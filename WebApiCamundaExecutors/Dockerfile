#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApiCamundaExecutors/WebApiCamundaExecutors.csproj", "WebApiCamundaExecutors/"]
COPY ["CamundaInterface/CamundaInterface.csproj", "CamundaInterface/"]
COPY ["CCFAProtocols/CCFAProtocols.csproj", "CCFAProtocols/"]
COPY ["UAMP/UAMP.csproj", "UAMP/"]
COPY ["ParserLibrary/ParserLibrary.csproj", "ParserLibrary/"]
COPY ["DMN_lib/DMN_lib.csproj", "DMN_lib/"]
COPY ["StandaloneKestrel/StandaloneKestrel.csproj", "StandaloneKestrel/"]
RUN dotnet restore "WebApiCamundaExecutors/WebApiCamundaExecutors.csproj"
COPY . .
WORKDIR "/src/WebApiCamundaExecutors"
RUN dotnet build "WebApiCamundaExecutors.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApiCamundaExecutors.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiCamundaExecutors.dll"]