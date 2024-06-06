#! /bin/bash
serviceName=hymson-mes-api-prod
docker service rm $serviceName
docker service create \
  --with-registry-auth \
  --name $serviceName \
  --mode global \
  --publish mode=host,published=30223,target=80 \
  --env ASPNETCORE_ENVIRONMENT=Production \
  --env TZ="Asia/Shanghai" \
  --env SERVICE_CHECK_HTTP=/health \
  --env SERVICE_NAME={{.Service.Name}} \
  --hostname="{{.Node.ID}}-{{.Service.Name}}"\
   --mount type=volume,src=hymsonvolume,dst=/logs \
   harbor.xnebula.com/new-energy/hymson.mes.api:20231109014106
