using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Neoledge.Nxc.Service.ApiConnector.Extensions.Options;
using Neoledge.Nxc.Service.ApiConnector.Handlers;
using Neoledge.Nxc.Service.ApiConnector.Imp;
using Neoledge.Nxc.Service.ApiConnector.Interfaces;
using Polly;
using Polly.Extensions.Http;

namespace Neoledge.Nxc.Service.ApiConnector.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiConnectorServices(this IServiceCollection services, Action<ApiConnectorOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.PostConfigure<ApiConnectorOptions>(ValidateClientServicesOptions);
            return services.RegisterServices();
        }

        public static IServiceCollection AddApiConnectorServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiConnectorOptions>(configuration.GetSection("ApiConnector"));
            services.PostConfigure<ApiConnectorOptions>(ValidateClientServicesOptions);
            return services.RegisterServices();
        }

        private static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // TODO : If needed
            //var retryPolicy = HttpPolicyExtensions
            //                 .HandleTransientHttpError()
            //                 .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            //var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10); // 10-second timeout

            //var circuitBreakerPolicy = HttpPolicyExtensions
            //    .HandleTransientHttpError()
            //    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)); // Break after 5 failures, reset after 30 sec

            services.AddTransient<KeycloakTokenHandler>();
            services.AddHttpClient<IMessageConnector, MessageConnector>((serviceProvider, httpClient) =>
            {
                var clientOptions = serviceProvider.GetRequiredService<IOptions<ApiConnectorOptions>>().Value;
                httpClient.BaseAddress = new Uri(clientOptions.BaseAddress);
                httpClient.Timeout = TimeSpan.FromMinutes(15);
            }).AddHttpMessageHandler<KeycloakTokenHandler>();

            //.AddPolicyHandler(retryPolicy)
            //.AddPolicyHandler(timeoutPolicy)
            //.AddPolicyHandler(circuitBreakerPolicy);

            return services;
        }

        private static void ValidateClientServicesOptions(ApiConnectorOptions clientOptions)
        {
            ArgumentNullException.ThrowIfNull(clientOptions);
            //TODO
        }
    }
}