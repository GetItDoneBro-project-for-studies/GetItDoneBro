FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS with-node
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs


FROM with-node AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/GetItDoneBro.Api/GetItDoneBro.Api.csproj", "src/GetItDoneBro.Api/"]
COPY ["src/GetItDoneBro.Application/GetItDoneBro.Application.csproj", "src/GetItDoneBro.Application/"]
COPY ["src/GetItDoneBro.Domain/GetItDoneBro.Domain.csproj", "src/GetItDoneBro.Domain/"]
COPY ["src/GetItDoneBro.Infrastructure/GetItDoneBro.Infrastructure.csproj", "src/GetItDoneBro.Infrastructure/"]
COPY ["src/aspire/GetItDoneBro.ServiceDefaults/GetItDoneBro.ServiceDefaults.csproj", "src/aspire/GetItDoneBro.ServiceDefaults/"]
COPY ["frontend/GetItDoneBro.FrontEnd/GetItDoneBro.FrontEnd.esproj", "frontend/GetItDoneBro.FrontEnd/"]
COPY ["Directory.Build.props", "Directory.Build.props"]
COPY ["Directory.Packages.props", "Directory.Packages.props"]
RUN dotnet restore "./src/GetItDoneBro.Api/GetItDoneBro.Api.csproj"
COPY . .
WORKDIR "/src/src/GetItDoneBro.Api"
RUN dotnet build "./GetItDoneBro.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GetItDoneBro.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GetItDoneBro.Api.dll"]
