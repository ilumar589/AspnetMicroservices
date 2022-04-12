# AspnetMicroservices
AspnetMicroservices

Run microservices:
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up -d
Stop all docker images:  docker stop $(docker ps -aq)
Remove all docker images:  docker remove $(docker ps -aq)
Remove unused images: docker system prune