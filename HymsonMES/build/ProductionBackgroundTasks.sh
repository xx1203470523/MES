#! /bin/bash
serviceName=hymson-mes-backgroundtasks-prod
docker service rm $serviceName
docker service create \
  --with-registry-auth \
  --name $serviceName \
  --replicas 2 \
  --env DOTNET_ENVIRONMENT=Production \
  --env TZ="Asia/Shanghai" \
  --env SERVICE_NAME={{.Service.Name}} \
  --hostname="{{.Node.ID}}-{{.Service.Name}}"\
   --mount type=volume,src=hymsonvolume,dst=/logs \
 10.10.79.13:8081/test/hymson.mes.backgroundtasks:20230512092656