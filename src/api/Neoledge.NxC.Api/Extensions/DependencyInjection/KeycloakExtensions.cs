using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Neoledge.Nxc.Domain.Interfaces.Api.Context;
using Neoledge.NxC.Api.Context;
using Neoledge.NxC.Api.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;

namespace Neoledge.NxC.Api.Extensions.DependencyInjection
{
    internal static class KeycloakExtensions
    {
        internal static IServiceCollection AddKeycloak(this IServiceCollection services, IConfiguration configuration)
        {
            //var keycloakOptions = configuration.Get<KeycloakOptions>();

            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();
            services.AddAuthorization();
            services
                .AddAuthentication()
                .AddKeycloakJwtBearer(
                    serviceName: "keycloak",
                    realm: "NxC",
                    options =>
                    {
                        options.Audience = "nxc";
                        options.RequireHttpsMetadata = false;
                        options.IncludeErrorDetails = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            RoleClaimType = ClaimTypes.Role // Maps to User.IsInRole()
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnTokenValidated = context =>
                            {
                                if (context.Principal?.Identity is ClaimsIdentity identity)
                                {
                                    // Add realm roles
                                    var realmAccess = context.Principal.FindFirst("realm_access")?.Value;
                                    if (!string.IsNullOrEmpty(realmAccess))
                                    {
                                        var realmRoles = JsonSerializer.Deserialize<KeycloakRealmAccess>(realmAccess)?.Roles;
                                        foreach (var role in realmRoles ?? Enumerable.Empty<string>())
                                        {
                                            identity.AddClaim(new Claim(ClaimTypes.Role, role));
                                        }
                                    }

                                   
                                    // Add client roles
                                    //var clientId = builder.Configuration["Keycloak:ClientId"];
                                    //var resourceAccess = context.Principal.FindFirst("resource_access")?.Value;
                                    //if (!string.IsNullOrEmpty(resourceAccess))
                                    //{
                                    //    var clientRoles = JsonSerializer.Deserialize<Dictionary<string, KeycloakClientAccess>>(resourceAccess);
                                    //    if (clientRoles?.TryGetValue(clientId, out var access) == true)
                                    //    {
                                    //        foreach (var role in access.Roles ?? Enumerable.Empty<string>())
                                    //        {
                                    //            identity.AddClaim(new Claim(ClaimTypes.Role, role));
                                    //        }
                                    //    }
                                    //}
                                }
                                return Task.CompletedTask;
                            }
                        };
                    });

            return services;
        }
    }
}