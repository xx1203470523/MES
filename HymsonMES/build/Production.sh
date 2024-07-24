#! /bin/bash
PublishEnvironment=Release
timestamp=$(date +%Y%m%d%H%M%S)
registryUrl=nuget.czhipu.com/xnebula
imageName=hymson.mes.api
sudo docker build  --build-arg PublishEnvironment=$PublishEnvironment  -t $imageName:$timestamp -f ./HymsonMES/src/Presentation/Hymson.MES.Api/Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp

imageName=hymson.mes.equipment.api
sudo docker build  --build-arg PublishEnvironment=$PublishEnvironment  -t $imageName:$timestamp -f ./HymsonMES/src/Presentation/Hymson.MES.Equipment.Api/Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp


imageName=hymson.mes.backgroundtasks
sudo docker build  --build-arg PublishEnvironment=$PublishEnvironment  -t $imageName:$timestamp -f ./HymsonMES/src/Presentation/Hymson.MES.BackgroundTasks/Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp


imageName=hymson.mes.system.api
sudo docker build  --build-arg PublishEnvironment=$PublishEnvironment  -t $imageName:$timestamp -f ./HymsonMES/src/Presentation/Hymson.MES.System.Api/Dockerfile .
sudo docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
sudo docker push $registryUrl/$imageName:$timestamp