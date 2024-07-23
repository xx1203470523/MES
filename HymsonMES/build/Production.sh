#! /bin/bash
PublishEnvironment=Release
timestamp=$(date +%Y%m%d%H%M%S)
serviceName=hymson-mes-api-prod
registryUrl=harbor.xnebula.com/new-energy
imageName=hymson.mes.api
docker build  --build-arg PublishEnvironment=$PublishEnvironment  -t $imageName:$timestamp -f ./HymsonMES/src/Presentation/Hymson.MES.Api/Dockerfile .
docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
docker push $registryUrl/$imageName:$timestamp