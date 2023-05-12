#! /bin/bash
serviceName=hymson-mes-backgroundtasks-prod
docker service rm $serviceName
docker service create \
  --name $serviceName \
  --replicas 2 \
  --env DOTNET_ENVIRONMENT=Production \
  --env TZ="Asia/Shanghai" \
  --env SERVICE_NAME={{.Service.Name}} \
  --hostname="{{.Node.ID}}-{{.Service.Name}}"\
   --mount type=volume,src=hymsonvolume,dst=/logs \
  $registryUrl/$imageName:$timestamp