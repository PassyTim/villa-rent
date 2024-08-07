FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["VillaRent.API/VillaRent.API.csproj", "VillaRent.API/"]
COPY ["VillaRent.Application/VillaRent.Application.csproj", "VillaRent.Application/"]
COPY ["VillaRent.Infrastructure/VillaRent.Infrastructure.csproj", "VillaRent.Infrastructure/"]
COPY ["VillaRent.Domain/VillaRent.Domain.csproj", "VillaRent.Domain/"]
COPY ["VillaRent.Persistence/VillaRent.Persistence.csproj", "VillaRent.Persistence/"]
RUN dotnet restore "VillaRent.API/VillaRent.API.csproj"
COPY . .
WORKDIR "/src/VillaRent.API"
RUN dotnet build "VillaRent.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "VillaRent.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "VillaRent.API.dll"]
