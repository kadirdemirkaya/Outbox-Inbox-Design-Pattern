#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Publisher/Presentation/Outbox.Order.API/Outbox.Order.API.csproj", "src/Publisher/Presentation/Outbox.Order.API/"]
COPY ["src/Publisher/Core/Outbox.Application/Outbox.Application.csproj", "src/Publisher/Core/Outbox.Application/"]
COPY ["src/Publisher/Outbox.Shared/Outbox.Shared.csproj", "src/Publisher/Outbox.Shared/"]
COPY ["src/Publisher/Core/Outbox.Domain/Outbox.Domain.csproj", "src/Publisher/Core/Outbox.Domain/"]
COPY ["src/Publisher/Infrastructure/Outbox.Persistence/Outbox.Persistence.csproj", "src/Publisher/Infrastructure/Outbox.Persistence/"]
RUN dotnet restore "./src/Publisher/Presentation/Outbox.Order.API/./Outbox.Order.API.csproj"
COPY . .
WORKDIR "/src/src/Publisher/Presentation/Outbox.Order.API"
RUN dotnet build "./Outbox.Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Outbox.Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Outbox.Order.API.dll"]