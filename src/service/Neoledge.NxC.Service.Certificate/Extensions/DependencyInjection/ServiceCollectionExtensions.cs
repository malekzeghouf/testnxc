using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neoledge.NxC.Service.Certificate.Extensions.Options;
using Neoledge.NxC.Service.Certificate.Generation;
using Neoledge.NxC.Service.Certificate.Serialization;
using Neoledge.NxC.Service.Certificate.Validation;

namespace Neoledge.NxC.Service.Certificate.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCertificateServices(this IServiceCollection services, Action<CertificateServiceOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.PostConfigure<CertificateServiceOptions>(ValidateCertificateServiceOptions);
            return services.RegisterServices();
        }

        public static IServiceCollection AddCertificateServices(this IServiceCollection services, IConfiguration namedConfigurationSection)
        {
            services.Configure<CertificateServiceOptions>(namedConfigurationSection);
            services.PostConfigure<CertificateServiceOptions>(ValidateCertificateServiceOptions);
            return services.RegisterServices();
        }

        private static void ValidateCertificateServiceOptions(CertificateServiceOptions options)
        {
            if (options.PbeMinimumPasswordLength < 0) throw new ArgumentException($"PbeMinimumPasswordLength must be greater or equal to zero.", nameof(options));
        }

        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<ICertificateGeneratorManager, CertificateGeneratorManager>();
            services.AddSingleton<ICertificateValidationManager, CertificateValidationManager>();
            services.AddSingleton<ICertificateSerializationManager, CertificateSerializationManager>();
            return services;
        }
    }
}