docker compose -f "docker-compose.yml" stop
docker compose -f "docker-compose.yml" rm --force
docker compose -f "docker-compose.yml" -f "docker-compose.yml" up --build corporatePortal.Database corporatePortal.Elsa