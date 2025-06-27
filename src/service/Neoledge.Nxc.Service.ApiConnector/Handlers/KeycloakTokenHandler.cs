using Microsoft.Extensions.Options;
using Neoledge.Nxc.Service.ApiConnector.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Neoledge.Nxc.Service.ApiConnector.Handlers
{
    public class KeycloakTokenHandler(IHttpClientFactory httpClientFactory, IOptions<ApiConnectorOptions> options) : DelegatingHandler
    {
        private string? _accessToken;
        private DateTime _expiresAt = DateTime.MinValue;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_accessToken == null || DateTime.UtcNow >= _expiresAt)
            {
                var httpClient = httpClientFactory.CreateClient();

                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "client_id", options.Value.ClientId },
                    { "client_secret", options.Value.ClientSecret },
                    { "grant_type", "client_credentials" }
                });

                var response = await httpClient.PostAsync(options.Value.TokenEndpoint, content, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var payload = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: cancellationToken).ConfigureAwait(false);
                _accessToken = payload.GetProperty("access_token").GetString();
                var expiresIn = payload.GetProperty("expires_in").GetInt32();
                _expiresAt = DateTime.UtcNow.AddSeconds(expiresIn - 60); // Renew before actual expiry
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}