IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresDatabaseResource> database = builder
    .AddPostgres("database")
    .WithImage("postgres:17")
    .WithBindMount("../../.containers/db", "/var/lib/postgresql/data")
    .AddDatabase("getitdonebro");

IResourceBuilder<KeycloakResource> keycloak = builder
    .AddKeycloak("keycloak", 8080);

IResourceBuilder<ProjectResource> api = builder.AddProject<Projects.GetItDoneBro_Api>("getitdonebro-api")
    .WithEnvironment("ConnectionStrings__Database", database)
    .WithReference(keycloak)
    .WithReference(database)
    .WaitFor(database);

builder
    .AddNpmApp("getitdonebro-frontend", "../GetItDoneBro.FrontEnd" , "dev")
    .WithHttpsEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .WithReference(keycloak)
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
