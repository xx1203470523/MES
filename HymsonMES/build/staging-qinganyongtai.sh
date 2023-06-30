#! /bin/bash
PublishEnvironment=Release
timestamp=$(date +%Y%m%d%H%M%S)
serviceName=hymson-mes-api-qinganyongtai-test
registryUrl=10.10.79.13:8081/test
imageName=hymson.mes.api
docker build  --build-arg PublishEnvironment=$PublishEnvironment  -t $imageName:$timestamp -f ./HymsonMES/src/Presentation/Hymson.MES.Api/Dockerfile .
docker tag $imageName:$timestamp  $registryUrl/$imageName:$timestamp
docker push $registryUrl/$imageName:$timestamp
docker service rm $serviceName
docker service create \
  --with-registry-auth \
  --name $serviceName \
  --mode global \
  --publish mode=host,published=20024,target=80 \
  --env ASPNETCORE_ENVIRONMENT=Staging \
  --env TZ="Asia/Shanghai" \
  --env SERVICE_CHECK_HTTP=/health \
  --env SERVICE_NAME={{.Service.Name}} \
  --hostname="{{.Node.ID}}-{{.Service.Name}}" \
   --mount type=volume,src=hymsonvolume,dst=/logs \
  $registryUrl/$imageName:$timestamp