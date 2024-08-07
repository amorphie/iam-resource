<# Docker up #>
cd ./etc/docker/

$networkName = "bbt-development"

$networkExists = docker network ls --filter name=^$networkName$ --format "{{.Name}}"

if ($networkExists -eq $null) {
    docker network create $networkName
} else {
    Write-Output "Network '$networkName' already exists."
}

docker compose up -d

cd ../..
 
<# Dapr run #>
dapr run --app-id "Resource" --app-port "4200" --components-path "./etc/Dapr/Components" --dapr-grpc-port "42011" --dapr-http-port "42010"
