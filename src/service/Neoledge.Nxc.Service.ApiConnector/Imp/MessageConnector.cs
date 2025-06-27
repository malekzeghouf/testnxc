using Neoledge.Nxc.Domain.Api.Client;
using Neoledge.Nxc.Domain.Api.Inbox;
using Neoledge.Nxc.Domain.Api.Member;
using Neoledge.Nxc.Domain.Api.Message;
using Neoledge.Nxc.Service.ApiConnector.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Neoledge.Nxc.Service.ApiConnector.Imp
{
    internal class MessageConnector(HttpClient httpClient) : IMessageConnector
    {
        public async Task<IList<MemberResponse>?> GetMembersAsync(CancellationToken cancellationToken)
        {
            return await httpClient.GetFromJsonAsync<IList<MemberResponse>>("/api/member", cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<InboxResponse>?> GetInboxesAsync(string memberId, CancellationToken cancellationToken)
        {
            return await httpClient.GetFromJsonAsync<IList<InboxResponse>>($"/api/inbox/{memberId}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<InboxResponse>?> GetMyInboxesAsync(CancellationToken cancellationToken)
        {
            return await httpClient.GetFromJsonAsync<IList<InboxResponse>>($"/api/inbox", cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> GetInboxPublicCertificateAsync(string inboxId, CancellationToken cancellationToken)
        {
            var result = await httpClient.GetStringAsync($"/api/inbox/certificate/{inboxId}", cancellationToken).ConfigureAwait(false);
            return result;
        }

        public async Task<bool> PublishPublicCertificateAsync(PublishPublicCertificateParameters parameters, CancellationToken cancellationToken)
        {
            var responseMessage = await httpClient.PostAsJsonAsync($"/api/inbox/certificate", parameters, cancellationToken).ConfigureAwait(false);
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<Guid> SendBodyAsync(Stream stream, CancellationToken cancellationToken)
        {
            using var multipart = new MultipartFormDataContent();
            using var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            multipart.Add(streamContent, "file", "archive.enc");

            var response = await httpClient.PostAsync("/api/message/send/body", multipart, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            SendBodyResponse? result = await response.Content.ReadFromJsonAsync<SendBodyResponse>(cancellationToken).ConfigureAwait(false);
            return result == null ? throw new InvalidOperationException("Failed to read response from SendBodyAsync.") : result.Guid;
        }

        public async Task<SendMessageResponse> SendMessageAsync(MessageSendParameters parameters, CancellationToken cancellationToken)
        {
            var response = await httpClient.PostAsJsonAsync("/api/message/send", parameters, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            SendMessageResponse? result = await response.Content.ReadFromJsonAsync<SendMessageResponse>(cancellationToken).ConfigureAwait(false);
            return result ?? throw new InvalidOperationException("Failed to read response from SendMessageAsync.");
        }

        public async Task<PeekMessageResponse?> PeekMessageAsync(CancellationToken cancellationToken)
        {
            var result = await httpClient.GetFromJsonAsync<PeekMessageResponse>("/api/message/peek", cancellationToken).ConfigureAwait(false);
            return result;
        }

        public async Task<Stream> ReceiveMessageBodyAsync(Guid messageId, CancellationToken cancellationToken)
        {
            var result = await httpClient.GetStreamAsync($"/api/message/receive/body/{messageId}", cancellationToken).ConfigureAwait(false);
            return result;
        }

        public async Task<ReceivedMessageResponse?> ReceiveMessageAsync(Guid messageId, CancellationToken cancellationToken)
        {
            var result = await httpClient.GetFromJsonAsync<ReceivedMessageResponse>($"/api/message/receive/{messageId}", cancellationToken).ConfigureAwait(false);
            return result;
        }

        public async Task<ReceivedMessageResponse?> AckMessageAsync(MessageStateUpateParameters parameters, CancellationToken cancellationToken)
        {
            var response = await httpClient.PutAsJsonAsync($"/api/message/receive/ack", parameters, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            ReceivedMessageResponse? result = await response.Content.ReadFromJsonAsync<ReceivedMessageResponse>(cancellationToken).ConfigureAwait(false);
            return result ?? throw new InvalidOperationException("Failed to read response from AckMessageAsync.");
        }


        
    }
}