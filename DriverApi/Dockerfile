#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DriverApi/DriverApi.csproj", "DriverApi/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "DriverApi/DriverApi.csproj"
COPY . .
WORKDIR "/src/DriverApi"
RUN dotnet build "DriverApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DriverApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DriverApi.dll"]
