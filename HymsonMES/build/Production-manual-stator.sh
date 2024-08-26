#! /bin/bash
PublishEnvironment=Release
sudo dotnet publish "HymsonMES/src/Presentation/Hymson.MES.BackgroundTasks.Stator/Hymson.MES.BackgroundTasks.Stator.csproj" -c ${PublishEnvironment} --runtime linux-x64 --no-self-contained -o /app/statorback /p:UseAppHost=false
