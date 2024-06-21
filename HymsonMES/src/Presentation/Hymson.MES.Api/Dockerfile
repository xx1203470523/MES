ARG PublishEnvironment
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
ARG PublishEnvironment
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG PublishEnvironment
WORKDIR /HymsonMES
COPY ["HymsonMES/", "HymsonMES/"]
FROM build AS publish
ARG PublishEnvironment
RUN echo ${PublishEnvironment}
RUN $version=date "+%y%m%d.%H.%M.%S"
RUN dotnet publish  "HymsonMES/src/Presentation/Hymson.MES.Api/Hymson.MES.Api.csproj" -p:Version=$(date "+%H.%M.%S.%Y+%m%d") -c ${PublishEnvironment} --runtime linux-x64 --no-self-contained -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hymson.MES.Api.dll"]