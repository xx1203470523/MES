#! /bin/bash
PublishEnvironment=Release
timestamp=$(date +%Y%m%d%H%M%S)
registryUrl=nuget.czhipu.com/xnebula
imageName=hymson.mes.backgroundtasks.nio
sudo docker build  -t $imageName:$timestamp -f ./nioback-Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp

imageName=hymson.mes.backgroundtasks.rotor
sudo docker build  -t $imageName:$timestamp -f ./rotorback-Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp


imageName=hymson.mes.backgroundtasks.stator
sudo docker build  -t $imageName:$timestamp -f ./statorback-Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp