using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Authentication.ClientFactory;
using FS.Keycloak.RestApiClient.Authentication.Flow;
using FS.Keycloak.RestApiClient.ClientFactory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Neoledge.NxC.Service.Certificate.Extensions.DependencyInjection;
using Neoledge.NxC.Service.Certificate.Extensions.Options;
using Neoledge.NxC.Service.Cryptography.Extensions.DependencyInjection;
using Neoledge.NxC.Service.Cryptography.Extensions.Options;
using Neoledge.NxC.Service.MinoStorage.Extensions.DependencyInjection;
using Neoledge.NxC.Service.NxC.Extensions.Options;
using Neoledge.NxC.Service.NxC.Imp;
using Neoledge.NxC.Service.NxC.Imp.Admin;
using Neoledge.NxC.Service.NxC.Interfaces;

namespace Neoledge.NxC.Service.NxC.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNxCServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.RegisterServices(configuration);
        }

        private static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));
            services.AddSingleton<ClientCredentialsFlow>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<KeycloakOptions>>().Value;
                return new ClientCredentialsFlow
                {
                    KeycloakUrl = options.BaseAdress,
                    Realm = options.Realm,
                    ClientId = options.ClientId,
                    ClientSecret = options.ClientSecret
                };
            });

            services.AddSingleton(sp =>
            {
                var credentials = sp.GetRequiredService<ClientCredentialsFlow>();
                var httpClient = AuthenticationHttpClientFactory.Create(credentials);
                return ApiClientFactory.Create<UsersApi>(httpClient);
            });
            services.AddSingleton(sp =>
            {
                var credentials = sp.GetRequiredService<ClientCredentialsFlow>();
                var httpClient = AuthenticationHttpClientFactory.Create(credentials);
                return ApiClientFactory.Create<OrganizationsApi>(httpClient);
            });

            // Minio storage services
            services.AddMinioStorageServices(configuration);

            services.AddCertificateServices(options =>
            {
                options = new CertificateServiceOptions();
            });
            services.AddCryptographyServices(options =>
            {
                options = new CryptographyServiceOptions();
            });

            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IInboxService, InboxService>();

            services.AddScoped<IFederationAdminService, FederationAdminService>();
            return services;
        }
    }
}