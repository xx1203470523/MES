#! /bin/bash
PublishEnvironment=Release
timestamp=$(date +%Y%m%d%H%M%S)
registryUrl=nuget.czhipu.com/xnebula
imageName=hymson.mes.api
sudo docker build  -t $imageName:$timestamp -f ./HymsonMES/build/mesapi-Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp

imageName=hymson.mes.equipment.api
sudo docker build  -t $imageName:$timestamp -f ./HymsonMES/build/equapi-Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp


imageName=hymson.mes.backgroundtasks
sudo docker build  -t $imageName:$timestamp -f ./HymsonMES/build/back-Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp


imageName=hymson.mes.system.api
sudo docker build  -t $imageName:$timestamp -f ./HymsonMES/build/sysapi-Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp