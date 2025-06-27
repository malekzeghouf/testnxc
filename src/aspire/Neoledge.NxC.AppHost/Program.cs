using Neoledge.Elise.NxC.Aspire.Minio.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Database services
var db = builder.AddConnectionString("DefaultConnection");

///var sql = builder.AddSqlServer("sql", port: 14329)
///                 .WithEndpoint(name: "sqlEndpoint", targetPort: 14330);

///var db = sql.AddDatabase("sqldata");

var keycloak = builder.AddKeycloak("keycloak", 8080).WithDataVolume().WithLifetime(ContainerLifetime.Persistent);

// MinIO services
var minioAccessKey = builder.AddParameter("minio-accessKey");
var minioSecretKey = builder.AddParameter("minio-secretKey");

var minio = builder.AddMinio("minio", apiPort: 9000, consolePort: 9001, accessKey: minioAccessKey, secretKey: minioSecretKey)
    .WithLifetime(ContainerLifetime.Persistent);

var migrations = builder.AddProject<Projects.Neoledge_NxC_DatabaseMigrationService>("migrations")
    .WithReference(db)
    .WaitFor(db);

// NeoXchange API
var platform = builder.AddProject<Projects.Neoledge_NxC_Api>("nxc-api")
     .WithReference(minio)
     .WithReference(db)
     .WithReference(keycloak)
     .WithEnvironment("Minio:AccessKey", minioAccessKey)
     .WithEnvironment("Minio:SecretKey", minioSecretKey)
     .WaitFor(minio)
     .WaitFor(keycloak)
     .WaitFor(migrations);

// CLIENT
/*var clientApi = builder.AddProject<Projects.Neoledge_NxC_Client_Api>("client-api")
    .WithExternalHttpEndpoints(); */

/*builder.AddNpmApp("client", "../../front/Neoledge.NxC.Client.Front", scriptName: "dev")
    .WithReference(clientApi)
    .WaitFor(clientApi)
    .WithHttpEndpoint(port: 3001, targetPort: 3001, isProxied: false)
    .WithExternalHttpEndpoints(); */

/*builder.AddProject<Projects.Neoledge_Nxc_Client_Batch>("batch")
    .WithReference(platform)
    .WaitFor(platform); */

builder.AddNpmApp("nxc", "../../front/Neoledge.NxC.Front", scriptName: "dev")
    .WithReference(platform)
    .WaitFor(platform)
    .WithEnvironment("NUXT_OIDC_PROVIDERS_KEYCLOAK_CLIENT_ID", "nxc")
    .WithEnvironment("NUXT_OIDC_PROVIDERS_KEYCLOAK_CLIENT_SECRET", "UGh7vKq79DrQgAPaf6STq4mIrknozyUu")
    .WithEnvironment("NUXT_OIDC_PROVIDERS_KEYCLOAK_BASE_URL", "http://localhost:8080/realms/NxC")
    .WithHttpEndpoint(port: 3000, targetPort: 3000, isProxied: false)
    .WithExternalHttpEndpoints();

await builder.Build().RunAsync();