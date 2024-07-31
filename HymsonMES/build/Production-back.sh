#! /bin/bash
PublishEnvironment=Release
timestamp=$(date +%Y%m%d%H%M%S)
registryUrl=nuget.czhipu.com/xnebula

imageName=hymson.mes.backgroundtasks.nio
sudo docker build  --build-arg PublishEnvironment=$PublishEnvironment  -t $imageName:$timestamp -f ./HymsonMES/src/Presentation/Hymson.MES.BackgroundTasks.NIO/Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp

imageName=hymson.mes.backgroundtasks.rotor
sudo docker build  --build-arg PublishEnvironment=$PublishEnvironment  -t $imageName:$timestamp -f ./HymsonMES/src/Presentation/Hymson.MES.BackgroundTasks.Rotor/Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp


imageName=hymson.mes.backgroundtasks.stator
sudo docker build  --build-arg PublishEnvironment=$PublishEnvironment  -t $imageName:$timestamp -f ./HymsonMES/src/Presentation/Hymson.MES.BackgroundTasks.Stator/Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp
