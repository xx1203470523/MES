#! /bin/bash
serviceName=hymson-mes-api-prod
docker service rm $serviceName
docker service create \
  --name $serviceName \
  --mode global \
  --publish mode=host,published=30223,target=80 \
  --env ASPNETCORE_ENVIRONMENT=Production \
  --env TZ="Asia/Shanghai" \
  --env SERVICE_CHECK_HTTP=/health \
  --env SERVICE_NAME={{.Service.Name}} \
  --hostname="{{.Node.ID}}-{{.Service.Name}}"\
   --mount type=volume,src=hymsonvolume,dst=/logs \
   10.10.79.13:8081/test/hymson.mes.api:20230512081322
