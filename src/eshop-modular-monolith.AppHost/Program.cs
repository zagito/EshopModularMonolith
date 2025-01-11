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

builder.AddProject<Projects.Api>("api")
    .WithReference(eshopDb)
    .WithReference(seq)
    .WithReference(redis)
    .WaitFor(eshopDb)
    .WaitFor(seq)
    .WaitFor(redis);


builder.Build().Run();
