var builder = DistributedApplication.CreateBuilder(args);


var eshopDbName = "eshopdb";
var eshopDb = builder.AddPostgres("eshop-db")
    .WithEnvironment("POSTGRES_DB", eshopDbName)
    .WithEnvironment("POSTGRES_PORT", "15432")
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase(eshopDbName);

builder.AddProject<Projects.Api>("api")
    //.WithHttpsEndpoint(port: 5050, name: "eshop")
    .WithReference(eshopDb)
    .WaitFor(eshopDb);

builder.Build().Run();
