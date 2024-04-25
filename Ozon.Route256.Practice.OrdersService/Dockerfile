#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["OrderService/Domain/Ozon.Route256.Practice.OrderService.Domain.csproj", "OrderService/Domain/"]
RUN dotnet restore "./OrderService/Domain/Ozon.Route256.Practice.OrderService.Domain.csproj"
WORKDIR "OrderService/Domain"
COPY ./OrderService/Domain .
RUN dotnet build "./Ozon.Route256.Practice.OrderService.Domain.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR /src
COPY ["OrderService/Application/Ozon.Route256.Practice.OrderService.Application.csproj", "OrderService/Application/"]
RUN dotnet restore "./OrderService/Application/Ozon.Route256.Practice.OrderService.Application.csproj"
WORKDIR "OrderService/Application"
COPY ./OrderService/Application .
RUN dotnet build "./Ozon.Route256.Practice.OrderService.Application.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR /src
COPY ["OrderService/Infrastructure/Ozon.Route256.Practice.OrderService.Infrastructure.csproj", "OrderService/Infrastructure/"]
RUN dotnet restore "./OrderService/Infrastructure/Ozon.Route256.Practice.OrderService.Infrastructure.csproj"
WORKDIR "OrderService/Infrastructure"
COPY ./OrderService/Infrastructure .
RUN dotnet build "./Ozon.Route256.Practice.OrderService.Infrastructure.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR /src
COPY ["Ozon.Route256.Practice.OrdersService/Ozon.Route256.Practice.OrderService.csproj", "Ozon.Route256.Practice.OrdersService/"]
RUN dotnet restore "./Ozon.Route256.Practice.OrdersService/Ozon.Route256.Practice.OrderService.csproj"
WORKDIR "Ozon.Route256.Practice.OrdersService"
COPY ./Ozon.Route256.Practice.OrdersService .
RUN dotnet build "./Ozon.Route256.Practice.OrderService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ozon.Route256.Practice.OrderService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ozon.Route256.Practice.OrderService.dll"]