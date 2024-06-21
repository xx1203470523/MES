
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
ARG PublishEnvironment
FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
ARG PublishEnvironment
WORKDIR /HymsonMES
COPY ["HymsonMES/", "HymsonMES/"]
RUN dotnet publish  -p:Version=$(date "+%H.%M.%S.%Y+%m%d")  "HymsonMES/src/Presentation/Hymson.MES.BackgroundTasks/Hymson.MES.BackgroundTasks.csproj" -c ${PublishEnvironment} --runtime linux-x64 --no-self-contained -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hymson.MES.BackgroundTasks.dll"]