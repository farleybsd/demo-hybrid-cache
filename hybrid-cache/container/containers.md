# Containers

## SQL Server

```bash
docker run -d --name hybrid-cache-sqlserver \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=HybridCache@2026" \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2022-latest
```

## Redis

```bash
docker run -d --name hybrid-cache-redis \
  -p 6379:6379 \
  redis:latest
```

## Connection strings (appsettings.json)

- BlogDb: `Server=localhost,1433;Database=BlogDb;User Id=sa;Password=HybridCache@2026;TrustServerCertificate=True`
- Redis: `localhost:6379`
