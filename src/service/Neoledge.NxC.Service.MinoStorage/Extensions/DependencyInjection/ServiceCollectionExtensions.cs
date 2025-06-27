using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Neoledge.NxC.Service.MinoStorage.Imp;
using Neoledge.NxC.Service.MinoStorage.Interfaces;

namespace Neoledge.NxC.Service.MinoStorage.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMinioStorageServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.RegisterServices(configuration);
        }

        private static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var minioEndpoint = configuration["ConnectionStrings:minio"] ?? string.Empty;
            var accessKey = configuration["Minio:AccessKey"];
            var secretKey = configuration["Minio:SecretKey"];

            services.AddMinio(client =>
            {
                client.WithEndpoint(new Uri(minioEndpoint));
                client.WithCredentials(accessKey, secretKey);
                client.WithSSL(false);
            });

            services.AddSingleton<IStorageService, StorageService>();

            return services;
        }
    }
}