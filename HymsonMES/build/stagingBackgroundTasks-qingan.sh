#! /bin/bash
PublishEnvironment=Release
timestamp=$(date +%Y%m%d%H%M%S)
serviceName=hymson-mes-backgroundtasks-qingan-test
registryUrl=10.10.79.13:8081/test
imageName=hymson.mes.backgroundtasks
docker build  --build-arg PublishEnvironment=$PublishEnvironment  -t $imageName:$timestamp -f ./HymsonMES/src/Presentation/Hymson.MES.BackgroundTasks/Dockerfile .
docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
docker push $registryUrl/$imageName:$timestamp
docker service rm $serviceName
docker service create \
  --with-registry-auth \
  --name $serviceName \
  --replicas 2 \
  --env DOTNET_ENVIRONMENT=Staging \
  --env TZ="Asia/Shanghai" \
  --env SERVICE_NAME={{.Service.Name}} \
   --mount type=volume,src=hymsonvolume,dst=/logs \
  $registryUrl/$imageName:$timestamp