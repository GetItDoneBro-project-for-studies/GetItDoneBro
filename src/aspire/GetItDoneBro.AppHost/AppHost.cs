using GetItDoneBro.AppHost;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var realmDataPath = Path.Combine(path1: Directory.GetCurrentDirectory(), path2: "data");

var keycloak = builder
    .AddKeycloak(name: "keycloak", port: 8080)
    .WithDataVolume()
    .WithRealmImport(realmDataPath);

var postgres = builder
    .AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();
var database = postgres.AddDatabase("Portal");

builder
    .AddProject<GetItDoneBro_Api>("portal-api")
    .WithHttpHealthCheck("api/health/live")
    .WithReference(database)
    .WithReference(keycloak)
    .WithKeycloakConfiguration(keycloakBuilder: keycloak, realmImportPath: realmDataPath)
    .WaitFor(database)
    .WaitFor(keycloak);


await builder.Build().RunAsync();
