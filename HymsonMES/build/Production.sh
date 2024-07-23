#! /bin/bash
PublishEnvironment=Release
timestamp=$(date +%Y%m%d%H%M%S)
serviceName=hymson-mes-api-prod
registryUrl=192.168.180.149:5000/xnebula
imageName=hymson.mes.api
sudo docker build  --build-arg PublishEnvironment=$PublishEnvironment  -t $imageName:$timestamp -f ./HymsonMES/src/Presentation/Hymson.MES.Api/Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp