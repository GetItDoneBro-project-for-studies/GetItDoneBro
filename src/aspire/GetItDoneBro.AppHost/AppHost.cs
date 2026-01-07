using GetItDoneBro.AppHost;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var realmDataPath = Path.Combine(path1: Directory.GetCurrentDirectory(), path2: "data");

var keycloak = builder
    .AddKeycloak(name: "keycloak", port: 8080)
    .WithDataVolume()
    .WithOtlpExporter()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRealmImport(realmDataPath);

var postgres = builder
    .AddPostgres("postgres")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin(c => c.WithLifetime(ContainerLifetime.Persistent));

var database = postgres.AddDatabase("DataBase");

var api = builder
    .AddProject<GetItDoneBro_Api>("api")
    .WithHttpsEndpoint(port: 5001)
    .WithHttpHealthCheck("/health")
    .WithReference(database)
    .WithReference(keycloak)
    .WithUrls(context =>
    {
        foreach (ResourceUrlAnnotation url in context.Urls)
        {
            url.DisplayLocation = UrlDisplayLocation.DetailsOnly;
        }

        context.Urls.Add(new ResourceUrlAnnotation
        {
            Url = "/scalar",
            DisplayText = "API Reference",
            Endpoint = context.GetEndpoint("https")
        });
    })
    .WithKeycloakConfiguration(keycloakBuilder: keycloak, realmImportPath: realmDataPath)
    .WaitFor(database)
    .WaitFor(keycloak);


#pragma warning disable ASPIRECERTIFICATES001
var frontend = builder
    .AddViteApp("frontend", "../../frontend/GetItDoneBro.FrontEnd")
    .WithHttpsEndpoint(8118, env: "PORT")
    .WaitFor(api)
    .WithReference(api)
    .WithReference(keycloak)
    .WithEnvironment("BROWSER", "none")
    .WithEnvironment("VITE_KEYCLOAK_REALM", "getitdonebro")
    .WithEnvironment("VITE_KEYCLOAK_CLIENT_ID", "getitdonebro-fe")
    .WithHttpsDeveloperCertificate();
#pragma warning restore ASPIRECERTIFICATES001


api.PublishWithContainerFiles(frontend, "wwwroot");


await builder.Build().RunAsync();
