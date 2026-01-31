# Comandos Docker para SQL Server

## Opção 1: Usando Docker Compose (Recomendado)

```bash
# Subir o container
docker-compose up -d

# Ver logs
docker-compose logs -f

# Parar o container
docker-compose down

# Parar e remover volumes (apaga os dados)
docker-compose down -v
```

## Opção 2: Comando Docker direto

```bash
# Criar e iniciar o container
docker run -d \
  --name plataforma-marcenaria-db \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=MinhaSenhaNova@2025!" \
  -e "MSSQL_PID=Developer" \
  -p 1433:1433 \
  -v sqlserver_data:/var/opt/mssql \
  --restart unless-stopped \
  mcr.microsoft.com/mssql/server:2022-latest

# Ver logs
docker logs -f plataforma-marcenaria-db

# Parar o container
docker stop plataforma-marcenaria-db

# Iniciar o container (se já existir)
docker start plataforma-marcenaria-db

# Remover o container
docker rm -f plataforma-marcenaria-db

# Remover o volume (apaga os dados)
docker volume rm sqlserver_data
```

## Verificar se está rodando

```bash
# Ver containers em execução
docker ps

# Ver logs do container
docker logs plataforma-marcenaria-db
```

## Criar o banco de dados

Após o container estar rodando, você pode criar o banco de dados executando:

```bash
# Via dotnet ef (se tiver migrations)
dotnet ef database update

# Ou conectando diretamente no SQL Server e executando:
# CREATE DATABASE five_marcenaria;
```

