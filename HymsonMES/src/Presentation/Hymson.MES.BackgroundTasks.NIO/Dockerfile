#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Presentation/Hymson.MES.BackgroundTasks.NIO/Hymson.MES.BackgroundTasks.NIO.csproj", "src/Presentation/Hymson.MES.BackgroundTasks.NIO/"]
COPY ["src/Libraries/Hymson.MES.BackgroundServices.NIO/Hymson.MES.BackgroundServices.NIO.csproj", "src/Libraries/Hymson.MES.BackgroundServices.NIO/"]
COPY ["src/Libraries/Hymson.MES.CoreServices/Hymson.MES.CoreServices.csproj", "src/Libraries/Hymson.MES.CoreServices/"]
COPY ["src/Libraries/Hymson.MES.Data/Hymson.MES.Data.csproj", "src/Libraries/Hymson.MES.Data/"]
COPY ["src/Libraries/Hymson.MES.Core/Hymson.MES.Core.csproj", "src/Libraries/Hymson.MES.Core/"]
COPY ["src/Libraries/Hymson.MES.HttpClients/Hymson.MES.HttpClients.csproj", "src/Libraries/Hymson.MES.HttpClients/"]
RUN dotnet restore "./src/Presentation/Hymson.MES.BackgroundTasks.NIO/Hymson.MES.BackgroundTasks.NIO.csproj"
COPY . .
WORKDIR "/src/src/Presentation/Hymson.MES.BackgroundTasks.NIO"
RUN dotnet build "./Hymson.MES.BackgroundTasks.NIO.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Hymson.MES.BackgroundTasks.NIO.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hymson.MES.BackgroundTasks.NIO.dll"]