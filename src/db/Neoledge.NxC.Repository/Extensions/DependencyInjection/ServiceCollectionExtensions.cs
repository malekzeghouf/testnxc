using Microsoft.Extensions.DependencyInjection;
using Neoledge.NxC.Repository.Imp;
using Neoledge.NxC.Repository.Interfaces;

namespace Neoledge.NxC.Repository.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IFederationRepository, FederationRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IInboxRepository,InboxRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            return services;
        }
    }
}