#! /bin/bash
PublishEnvironment=Release
sudo dotnet publish "HymsonMES/src/Presentation/Hymson.MES.Api/Hymson.MES.Api.csproj" -c ${PublishEnvironment} --runtime linux-x64 --no-self-contained -o /app/mesapi /p:UseAppHost=false
sudo dotnet publish "HymsonMES/src/Presentation/Hymson.MES.Equipment.Api/Hymson.MES.Equipment.Api.csproj" -c ${PublishEnvironment} --runtime linux-x64 --no-self-contained -o /app/equapi /p:UseAppHost=false
sudo dotnet publish "HymsonMES/src/Presentation/Hymson.MES.System.Api/Hymson.MES.System.Api.csproj" -c ${PublishEnvironment} --runtime linux-x64 --no-self-contained -o /app/sysapi /p:UseAppHost=false
sudo dotnet publish "HymsonMES/src/Presentation/Hymson.MES.BackgroundTasks/Hymson.MES.BackgroundTasks.csproj" -c ${PublishEnvironment} --runtime linux-x64 --no-self-contained -o /app/mesback /p:UseAppHost=false

