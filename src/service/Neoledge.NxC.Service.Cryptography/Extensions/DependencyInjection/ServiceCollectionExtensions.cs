using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neoledge.NxC.Service.Cryptography.Extensions.Options;
using Neoledge.NxC.Service.Cryptography.Interfaces.Internal.Cipher;
using Neoledge.NxC.Service.Cryptography.Interfaces.Internal.Hash;
using Neoledge.NxC.Service.Cryptography.Interfaces.Internal.SessionKey;
using Neoledge.NxC.Service.Cryptography.Internal.Cipher;
using Neoledge.NxC.Service.Cryptography.Internal.Hash;
using Neoledge.NxC.Service.Cryptography.Internal.SessionKey;
using Neoledge.NxC.Service.Cryptography.Message;

namespace Neoledge.NxC.Service.Cryptography.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCryptographyServices(this IServiceCollection services, Action<CryptographyServiceOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.PostConfigure<CryptographyServiceOptions>(ValidateCryptoServicesOptions);
            return services.RegisterServices();
        }
        public static IServiceCollection AddCryptographyServices(this IServiceCollection services, IConfiguration namedConfigurationSection)
        {
            services.Configure<CryptographyServiceOptions>(namedConfigurationSection);
            services.PostConfigure<CryptographyServiceOptions>(ValidateCryptoServicesOptions);
            return services.RegisterServices();
        }

        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IAsymmetricCipherManager,AsymmetricCipherManager>();
            services.AddSingleton<ISymmetricCipherManager,SymmetricCipherManager>();
            services.AddSingleton<IHashManager,HashManager>();
            services.AddSingleton<ISessionKeyManager, SessionKeyManager>();

            services.AddSingleton<IMessageManager, MessageManager>();
            return services;
        }

        private static void ValidateCryptoServicesOptions(CryptographyServiceOptions cryptographyOptions)
        {
            ArgumentNullException.ThrowIfNull(cryptographyOptions);
            //TODO
        }
    }
}
