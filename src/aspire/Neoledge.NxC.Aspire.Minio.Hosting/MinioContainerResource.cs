using Aspire.Hosting.ApplicationModel;

namespace Neoledge.Elise.NxC.Aspire.Minio.Hosting
{
    /// <summary>
    /// A resource that represents a MinIO container.
    /// </summary>
    /// <param name="name">The name of the resource.</param>
    public class MinioContainerResource(string name, ParameterResource accessKey, ParameterResource secretKey)
        : ContainerResource(name),
        IResourceWithEndpoints,
        IResourceWithConnectionString,
        IResourceWithEnvironment,
        IResourceWithArgs
    {
        /// <summary>
        /// Gets the MinIO Access Key.
        /// </summary>
        public ParameterResource AccessKeyParameter { get; } = accessKey;

        /// <summary>
        /// Gets the MinIO Secret Key.
        /// </summary>
        public ParameterResource SecretKeyParameter { get; } = secretKey;

        /// <summary>
        /// Gets the endpoint for the MinIO API.
        /// </summary>
        public EndpointReference? ApiEndpoint { get; internal set; }

        /// <summary>
        /// Gets the endpoint for the MinIO Console.
        /// </summary>
        public EndpointReference? ConsoleEndpoint { get; internal set; }

        /// <summary>
        /// Gets the collection of endpoints for the MinIO resource.
        /// </summary>
        public IReadOnlyDictionary<string, EndpointReference> Endpoints =>
            new Dictionary<string, EndpointReference>
            {
                { "api", ApiEndpoint! },
                { "console", ConsoleEndpoint! }
            };

        public ReferenceExpression ConnectionStringExpression => ReferenceExpression.Create($"{ApiEndpoint}");

    }
}