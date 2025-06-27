using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Neoledge.Elise.NxC.Aspire.Minio.Hosting
{
    public static class MinioHostBuilderExtensions
    {
        /// <summary>
        /// Adds a MinIO container to the application model.
        /// </summary>
        /// <param name="builder">The <see cref="IDistributedApplicationBuilder"/>.</param>
        /// <param name="name">The name of the resource.</param>
        /// <param name="apiPort">The host port for the MinIO API (default: 9000).</param>
        /// <param name="consolePort">The host port for the MinIO Console (default: 9001).</param>
        /// <param name="accessKey">The access key for MinIO. If not provided, a random one will be generated.</param>
        /// <param name="secretKey">The secret key for MinIO. If not provided, a random one will be generated.</param>
        /// <returns>A reference to the <see cref="IResourceBuilder{MinioResource}"/>.</returns>
        public static IResourceBuilder<MinioContainerResource> AddMinio(
            this IDistributedApplicationBuilder builder,
            string name,
            IResourceBuilder<ParameterResource> accessKey,
            IResourceBuilder<ParameterResource> secretKey,
            int? apiPort = null,
            int? consolePort = null)
        {

            // Create a new MinioResource instance
            var minioResource = new MinioContainerResource(name, accessKey.Resource, secretKey.Resource);
         

            // Add the MinioResource to the application builder as a container
            var minioBuilder = builder.AddResource(minioResource)
                .WithImage("minio/minio", "latest")
                // Set environment variables for MinIO root user and password
                .WithEnvironment("MINIO_ADDRESS", ":9000") // Default MinIO API port
                .WithEnvironment("MINIO_CONSOLE_ADDRESS", ":9001") // Default MinIO Console port
                .WithEnvironment("MINIO_ROOT_USER", accessKey)
                .WithEnvironment("MINIO_ROOT_PASSWORD", secretKey)
                // Define the command to start the MinIO server
                .WithArgs("server", "/data", "--console-address", ":9001")
                .WithHttpEndpoint(targetPort: 9000, port: apiPort, name: "api")
                .WithHttpEndpoint(targetPort: 9001, port: consolePort, name: "console");

            // Configure the endpoints for the MinioResource
            minioResource.ApiEndpoint = minioBuilder.GetEndpoint("api");
            minioResource.ConsoleEndpoint = minioBuilder.GetEndpoint("console");

            return minioBuilder;
        }
    }
}