IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresDatabaseResource> database = builder
    .AddPostgres("database")
    .WithImage("postgres:17")
    .WithPgAdmin()
    .AddDatabase("getitdonebro");

IResourceBuilder<KeycloakResource> keycloak = builder
    .AddKeycloak("keycloak", 8081)
    .WithDataVolume()
    .WithRealmImport("../realms");

IResourceBuilder<ProjectResource> api = builder.AddProject<Projects.GetItDoneBro_Api>("getitdonebro-api")
    .WithEnvironment("ConnectionStrings__Database", database)
    .WithReference(keycloak)
    .WithReference(database)
    .WaitFor(database)
    .WaitFor(keycloak);

builder
    .AddNpmApp("getitdonebro-frontend", "../../frontend/GetItDoneBro.FrontEnd", "dev")
    .WithHttpsEndpoint(port: 5173, env: "PORT")
    .WithExternalHttpEndpoints()
    .WithReference(keycloak)
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
