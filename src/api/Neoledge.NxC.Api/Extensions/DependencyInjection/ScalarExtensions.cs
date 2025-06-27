using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace Neoledge.NxC.Api.Extensions.DependencyInjection
{
    internal static class ScalarExtensions
    {
        internal static IServiceCollection AddScalar(this IServiceCollection services, List<ApiVersion> apiVersions)
        {
            //var keycloakOptions = services
            //.BuildServiceProvider()
            //.GetRequiredService<IConfiguration>()
            //.Get<KeycloakOptions>();

            foreach (var version in apiVersions)
            {
                services.Configure<ScalarOptions>(options => options.AddDocument($"v{version.MajorVersion}", $"v{version.MajorVersion}"));
                services.AddOpenApi($"v{version.MajorVersion}", options =>
                {
                    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
                    options.UseOAuth2Authentication();

                    options.AddDocumentTransformer((document, context, ct) =>
                    {
                        var provider = context.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                        var description = provider.ApiVersionDescriptions.FirstOrDefault(d => d.GroupName == context.DocumentName);

                        document.Info = new OpenApiInfo
                        {
                            Title = "Neoledge NxC",
                            Version = description?.ApiVersion.ToString() ?? context.DocumentName,
                            Description = description?.IsDeprecated == true ? "This API version is deprecated." : null
                        };

                        return Task.CompletedTask;
                    });
                });
            }

            return services;
        }

        private static OpenApiOptions UseOAuth2Authentication(this OpenApiOptions options)
        {
            var bearerScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Description = "OAuth2 authentication using Keycloak.",
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("http://localhost:8080/realms/NxC/protocol/openid-connect/auth"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "openid" },
                            { "profile", "profile" }
                        }
                    }
                    
                },
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
          

            options.AddDocumentTransformer((document, context, ct) =>
            {
                document.Components ??= new();
                document.Components.SecuritySchemes.Add("Bearer", bearerScheme);
               
                return Task.CompletedTask;
            });
            options.AddOperationTransformer((operation, context, ct) =>
            {
                if (context.Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>().Any())
                {
                    operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new[] { "openid", "profile" }
                        }
                    }
                };
                }

                return Task.CompletedTask;
            });
            return options;
        }

    }
}