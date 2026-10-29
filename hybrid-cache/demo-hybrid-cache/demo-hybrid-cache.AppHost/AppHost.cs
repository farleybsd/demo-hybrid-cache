var builder = DistributedApplication.CreateBuilder(args);

// Containers persistentes: sobrevivem entre execuções do AppHost (o SQL Server demora para subir)
var sql = builder.AddSqlServer("sqlserver")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var blogDb = sql.AddDatabase("BlogDb");

var redis = builder.AddRedis("redis")
    .WithLifetime(ContainerLifetime.Persistent);

// Os nomes das connection strings ("BlogDb" e "Redis") são os mesmos lidos pela API
builder.AddProject<Projects.hybrid_cache_Api>("hybrid-cache-api")
    .WithReference(blogDb)
    .WithReference(redis, "Redis")
    .WaitFor(blogDb)
    .WaitFor(redis)
    .WithUrlForEndpoint("http", url => url.Url = "/swagger")
    .WithUrlForEndpoint("https", url => url.Url = "/swagger");

builder.Build().Run();
