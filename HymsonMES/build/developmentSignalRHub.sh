#! /bin/bash
PublishEnvironment=Debug
timestamp=$(date +%Y%m%d%H%M%S)
serviceName=hymson-mes-api-dev
registryUrl=harbor.xnebula.com/new-energy
imageName=hymson.mes.signalrhub
docker build --build-arg PublishEnvironment=$PublishEnvironment -t $imageName:$timestamp -f ./HymsonMES/src/Presentation/Hymson.MES.SignalRHub/Dockerfile .
docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
docker push $registryUrl/$imageName:$timestamp
docker service rm $serviceName
docker service create \
  --with-registry-auth \
  --name $serviceName \
  --mode global \
  --publish mode=host,published=10233,target=80 \
  --env ASPNETCORE_ENVIRONMENT=Development \
  --env TZ="Asia/Shanghai" \
  --env SERVICE_CHECK_HTTP=/health \
  --env SERVICE_NAME={{.Service.Name}} \
  --hostname="{{.Node.ID}}-{{.Service.Name}}"\
   --mount type=volume,src=hymsonvolume,dst=/logs \
  $registryUrl/$imageName:$timestamp