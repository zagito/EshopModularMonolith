var builder = DistributedApplication.CreateBuilder(args);

var eshopDbName = "eshopdb";
var eshopDb = builder.AddPostgres("eshop-db")
    .WithEnvironment("POSTGRES_DB", eshopDbName)
    .WithEnvironment("POSTGRES_PORT", "15432")
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase(eshopDbName);

var seq = builder.AddSeq("seq")
    .WithDataVolume()
    .ExcludeFromManifest()
    .WithLifetime(ContainerLifetime.Persistent);

var redis = builder.AddRedis("redis")
    .WithDataVolume();

var rabbitMq = builder.AddRabbitMQ("eshop-mq")
    .WithManagementPlugin();

var keycloak = builder.AddKeycloak("keycloak", 8080)
    .WithDataVolume()
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Api>("api")
    .WithReference(eshopDb)
    .WithReference(seq)
    .WithReference(redis)
    .WithReference(rabbitMq)
    .WithReference(keycloak)
    .WaitFor(eshopDb)
    .WaitFor(seq)
    .WaitFor(redis)
    .WaitFor(rabbitMq)
    .WaitFor(keycloak);

builder.Build().Run();
