#! /bin/bash
PublishEnvironment=Release
sudo dotnet publish "HymsonMES/src/Presentation/Hymson.MES.BackgroundTasks.NIO/Hymson.MES.BackgroundTasks.NIO.csproj" -c ${PublishEnvironment} --runtime linux-x64 --no-self-contained -o /app/nioback /p:UseAppHost=false
sudo dotnet publish "HymsonMES/src/Presentation/Hymson.MES.BackgroundTasks.Rotor/Hymson.MES.BackgroundTasks.Rotor.csproj" -c ${PublishEnvironment} --runtime linux-x64 --no-self-contained -o /app/rotorback /p:UseAppHost=false
sudo dotnet publish "HymsonMES/src/Presentation/Hymson.MES.BackgroundTasks.Stator/Hymson.MES.BackgroundTasks.Stator.csproj" -c ${PublishEnvironment} --runtime linux-x64 --no-self-contained -o /app/statorback /p:UseAppHost=false
