IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresDatabaseResource> database = builder
    .AddPostgres("database")
    .WithImage("postgres:17")
    .WithPgAdmin()
    .AddDatabase("getitdonebro");

var keycloak = builder
    .AddKeycloakContainer("keycloak")
    .WithDataVolume();

IResourceBuilder<KeycloakRealmResource> realm = keycloak.AddRealm("getitdonebro-realm");

IResourceBuilder<ProjectResource> api = builder.AddProject<Projects.GetItDoneBro_Api>("getitdonebro-api")
    .WithEnvironment("ConnectionStrings__Database", database)
    .WithReference(realm)
    .WithReference(database)
    .WaitFor(database)
    .WaitFor(keycloak);

builder
    .AddNpmApp("getitdonebro-frontend", "../../frontend/GetItDoneBro.FrontEnd", "dev")
    .WithHttpsEndpoint(port: 5173, env: "PORT")
    .WithExternalHttpEndpoints()
    .WithReference(realm)
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
