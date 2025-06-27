using Mapster;
using Neoledge.Nxc.Domain.Api.Message;
using Neoledge.Nxc.Domain.Database.Enum;
using Neoledge.NxC.Database.Models;
using Neoledge.NxC.Repository.Interfaces;
using Neoledge.NxC.Service.Certificate.Serialization;
using Neoledge.NxC.Service.Cryptography.Message;
using Neoledge.NxC.Service.MinoStorage.Interfaces;
using Neoledge.NxC.Service.NxC.Interfaces;
using System.Security.Cryptography.X509Certificates;

namespace Neoledge.NxC.Service.NxC.Imp
{
    public class MessageService(
        IStorageService storageService,
        IMessageRepository messageRepository,
        IInboxRepository inboxRepository,
        IMessageManager messageManager,
        ICertificateSerializationManager certificateSerializationManager) : IMessageService
    {
        public Task<Guid> SendBodyAsync(Stream file, string contentType, CancellationToken cancellationToken)
        {
            return SendBodyInternalAsync(file, contentType, cancellationToken);
        }

        private async Task<Guid> SendBodyInternalAsync(Stream file, string contentType, CancellationToken cancellationToken)
        {
            return await storageService.UploadFileToPublicBucketAsync(file, contentType, cancellationToken).ConfigureAwait(false);
        }

        public Task<Guid> SendAsync(MessageSendParameters parameters, string senderInboxId, string senderMemberId, string federationId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(parameters);
            ArgumentException.ThrowIfNullOrEmpty(senderInboxId);
            ArgumentException.ThrowIfNullOrEmpty(senderMemberId);
            ArgumentException.ThrowIfNullOrEmpty(federationId);
            return SendInternalAsync(parameters, senderInboxId, senderMemberId, federationId, cancellationToken);
        }

        private async Task<Guid> SendInternalAsync(MessageSendParameters parameters, string senderInboxId, string senderMemberId, string federationId, CancellationToken cancellationToken)
        {
            string recipientInboxId = parameters.Recipient.Split('@')[0];
            string recipientMemberId = parameters.Recipient.Split('@')[1].Split(".")[0];

            using var encredtedFile = new MemoryStream();
            await storageService.GetFileFromPublicAsync(parameters.MessageId, encredtedFile, cancellationToken).ConfigureAwait(false);

            string senderPem = await inboxRepository.GetInboxPublicCertificateAsync(senderInboxId, cancellationToken).ConfigureAwait(false);
            X509Certificate2 senderPublicCert = certificateSerializationManager.CreateFromPem(senderPem);

            string recipientPem = await inboxRepository.GetInboxPublicCertificateAsync(recipientInboxId, cancellationToken).ConfigureAwait(false);
            X509Certificate2 recipientPublicCert = certificateSerializationManager.CreateFromPem(recipientPem);

            var result = await messageManager.VerifyMessageAsync(senderPublicCert, parameters.MessageSeal.Adapt<MessageSceal>(), encredtedFile, cancellationToken).ConfigureAwait(false);

            if (result.VerificationResultCode != VerificationResultCode.Ok)
            {
                //TODO: handle error
            }

            var messageHeader = new MessageHeader
            {
                FederationId = federationId,
                RecipientMemberId = recipientMemberId,
                SenderMemberId = senderMemberId,
                SendUserCn = parameters.SenderInfo.UserCn,
                SendUserId = parameters.SenderInfo.UserId,
                //Id = Guid.NewGuid(),
                BodySize = encredtedFile.Length,
                SignedHash = parameters.MessageSeal.SignedFileHash,
                EncryptedKey = parameters.MessageSeal.EncryptedKey,
                EncryptedIV = parameters.MessageSeal.EncryptedIv,
                SenderCertificateThumbprint = Convert.FromBase64String(senderPublicCert.Thumbprint),
                RecipientCertificateThumbprint = Convert.FromBase64String(recipientPublicCert.Thumbprint),
                RecipientInboxId = recipientInboxId
            };

            await messageRepository.SaveAsync(messageHeader, cancellationToken).ConfigureAwait(false);
            var messageId = messageHeader.Id;
            // TODO: change message Expiration to a configurable value from appsettings.json or user choice
            var messageState = new MessageState
            {
                MessageId = messageId,
                Status = MessageStatus.Pending,
                Expiration = DateTime.UtcNow.AddDays(7), // Set expiration to 7 days from now
                LeasedUtcAt = null,
                LeasedUtcUntil = null
            };
            await messageRepository.InitMessageStateAsync(messageState, cancellationToken).ConfigureAwait(false);
            // TODO: change LocalIdentifier to dynamic value
            // TODO: check StatusMessage and missing properties in MessageHistory
            var messageHistory = new MessageHistory
            {
                MessageId = messageId,
                EventCode = MessageEventCode.Init,
                StatusMessage = MessageStatus.Pending.ToString(),
                LocalIdentifier = "",
            };
            await messageRepository.AddMessageHistoryAsync(messageHistory, cancellationToken).ConfigureAwait(false);

            await storageService.TransferFileToInboxAsync(parameters.MessageId, federationId, recipientMemberId, recipientInboxId, messageId.ToString(), cancellationToken).ConfigureAwait(false);

            return messageId;
        }

        public async Task<Guid?> PeekMessageAsync(string memberId, string federationId, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(memberId);
            ArgumentException.ThrowIfNullOrEmpty(federationId);
            return await PeekMessageInternalAsync(memberId, federationId, cancellationToken).ConfigureAwait(false);
        }

        private async Task<Guid?> PeekMessageInternalAsync(string memberId, string federationId, CancellationToken cancellationToken)
        {
            var messageId = await messageRepository.GetLastMessageAsync(memberId, federationId, cancellationToken).ConfigureAwait(false);
            return messageId;
        }

        public Task<bool> GetMessageBodyAsync(Guid messageId, Stream file, CancellationToken cancellationToken)
        {
            return GetMessageBodyInternalAsync(messageId, file, cancellationToken);
        }

        private async Task<bool> GetMessageBodyInternalAsync(Guid messageId, Stream file, CancellationToken cancellationToken)
        {
            var message = await messageRepository.GetMessageByIdAsync(messageId, cancellationToken).ConfigureAwait(false);

            await storageService.GetBodyFromBucketAsync(file, message.FederationId, message.RecipientMemberId, message.RecipientInboxId, message.Id.ToString(), cancellationToken).ConfigureAwait(false);

            return true;
        }

        public async Task<ReceivedMessageResponse> ReceivedMessageAsync(Guid messageId, CancellationToken cancellationToken)
        {
            return await ReceivedMessageAsyncInternal(messageId, cancellationToken).ConfigureAwait(false);
        }

        private async Task<ReceivedMessageResponse> ReceivedMessageAsyncInternal(Guid messageId, CancellationToken cancellationToken)
        {
            MessageHeader messageHeader = await messageRepository.GetMessageByIdAsync(messageId, cancellationToken).ConfigureAwait(false);
            Inbox receiverInbox = await inboxRepository.GetInboxeByIdAsync(messageHeader.RecipientInboxId, cancellationToken).ConfigureAwait(false);

            var messageBodySeal = new MessageSceal
            {
                SignedFileHash = messageHeader.SignedHash,
                EncryptedKey = messageHeader.EncryptedKey,
                EncryptedIv = messageHeader.EncryptedIV
            };

            var receivedMessage = new ReceivedMessageResponse
            {
                MessageId = messageHeader.Id,
                InboxId = messageHeader.RecipientInboxId,
                MessageBodySize = messageHeader.BodySize,
                MessageBodySeal = messageBodySeal.Adapt<MessageSealParameters>(),
                MemberId = messageHeader.SenderMemberId,
                MemberName = messageHeader.SendUserCn,
                InboxParameters = receiverInbox.InboxParameters.ToDictionary(i => i.Name, i => i.Value),
                MessageLastReceivedUtcDate = DateTime.Now,
                MessageReceivedCount = +1,
            };
            await messageRepository.UpdateMessageStateAsync(messageId, MessageStatus.Receiving, cancellationToken).ConfigureAwait(false);
            return receivedMessage;
        }

        public Task<ReceivedMessageResponse> AckMessageAsync(MessageStateUpateParameters parameters, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(parameters);
            return HundleMessageStatusAsyncInternal(parameters, MessageStatus.Received, cancellationToken);
        }

        public Task<ReceivedMessageResponse> NaKMessageAsync(MessageStateUpateParameters parameters, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(parameters);
            return HundleMessageStatusAsyncInternal(parameters, MessageStatus.Pending, cancellationToken);
        }

        public Task<ReceivedMessageResponse> RejectMessageAsync(MessageStateUpateParameters parameters, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(parameters);
            return HundleMessageStatusAsyncInternal(parameters, MessageStatus.Rejected, cancellationToken);
        }

        private async Task<ReceivedMessageResponse> HundleMessageStatusAsyncInternal(MessageStateUpateParameters parameters, MessageStatus status, CancellationToken cancellationToken)
        {
            MessageHeader messageHeader = await messageRepository.GetMessageByIdAsync(parameters.MessageId, cancellationToken).ConfigureAwait(false);

            await messageRepository.UpdateMessageStateAsync(parameters.MessageId, status, cancellationToken).ConfigureAwait(false);

            MessageHistory messageHistory = new()
            {
                LocalIdentifier = parameters.LocalIdentifier ?? string.Empty,
                EventCode = status switch
                {
                    MessageStatus.Received => MessageEventCode.Ack,
                    MessageStatus.Pending => MessageEventCode.Nak,
                    MessageStatus.Rejected => MessageEventCode.Reject,
                    _ => throw new InvalidOperationException($"Unsupported message status: {status}")
                },
                MessageId = messageHeader.Id,
                StatusMessage = status.ToString(),
            };
            await messageRepository.AddMessageHistoryAsync(messageHistory,cancellationToken).ConfigureAwait(false);

            var messageBodySeal = new MessageSceal
            {
                SignedFileHash = messageHeader.SignedHash,
                EncryptedKey = messageHeader.EncryptedKey,
                EncryptedIv = messageHeader.EncryptedIV
            };
            var receivedMessage = new ReceivedMessageResponse
            {
                MessageId = messageHeader.Id,
                InboxId = messageHeader.RecipientInboxId,
                MessageBodySize = messageHeader.BodySize,
                MessageBodySeal = messageBodySeal.Adapt<MessageSealParameters>(),
                MemberId = messageHeader.SenderMemberId,
                MemberName = messageHeader.SendUserCn,
                InboxParameters = new Dictionary<string, string>(),
                MessageLastReceivedUtcDate = DateTime.Now,
                MessageReceivedCount = +1,
            };
            return receivedMessage;
        }
    }
}