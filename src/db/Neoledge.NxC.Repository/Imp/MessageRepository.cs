using Microsoft.EntityFrameworkCore;
using Neoledge.Nxc.Domain.Database.Enum;
using Neoledge.Nxc.Domain.Exceptions;
using Neoledge.NxC.Database;
using Neoledge.NxC.Database.Models;
using Neoledge.NxC.Repository.Interfaces;

namespace Neoledge.NxC.Repository.Imp
{
    public class MessageRepository(IAppDbContext context) : IMessageRepository
    {
        public async Task SaveAsync(MessageHeader message, CancellationToken cancellationToken)
        {
            await context.MessageHeaders.AddAsync(message, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<MessageHeader> GetMessageByIdAsync(Guid messageId, CancellationToken cancellationToken)
        {
            var message = await context.MessageHeaders.FindAsync(messageId, cancellationToken).ConfigureAwait(false);
            if (message == null)
            {
                throw new EntityNotFoundException(nameof(MessageHeader), messageId.ToString());
            }
            return message;
        }

        public async Task<MessageHeader?> GetLastMessage(string inboxId, CancellationToken cancellationToken)
        {
            var lastMessage = await context.MessageHeaders
                                            .Where(m => m.RecipientInboxId == inboxId)
                                            .OrderByDescending(m => m.CreatedAt)
                                            .Select(m => m)
                                            .FirstOrDefaultAsync(cancellationToken);
            return lastMessage;
        }

        public async Task<Guid?> GetLastMessageAsync(string memberId, string federationId, CancellationToken cancellationToken)
        {
            var lastMessageId = await context.MessageHeaders.Include(m=>m.MessageState)
                .Where(m =>
                    m.RecipientMemberId == memberId &&
                    m.FederationId == federationId &&
                    m.MessageState.Status == MessageStatus.Pending)
                .OrderByDescending(m => m.CreatedAt)
                .Select(m => (Guid?)m.Id)
                .FirstOrDefaultAsync(cancellationToken);

            return lastMessageId;
        }

        public async Task InitMessageStateAsync(MessageState messageState, CancellationToken cancellationToken)
        {
            await context.MessageStates.AddAsync(messageState, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        public async Task UpdateMessageStateAsync(Guid messageId, MessageStatus status, CancellationToken cancellationToken)
        {
            var messageState = await context.MessageStates.Where(ms => ms.MessageId == messageId).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            if(messageState == null)
            {
                throw new EntityNotFoundException(nameof(MessageState), messageId.ToString());
            }
            messageState.Status = status;
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task AddMessageHistoryAsync(MessageHistory messageHistory, CancellationToken cancellationToken)
        {
            await context.MessageHistories.AddAsync(messageHistory,cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<MessageState>> GetExpiredLeasedMessagesAsync(DateTime now)
        {
            return await context.MessageHeaders.Where(mh => mh.MessageState.Status == MessageStatus.Receiving
                  && mh.MessageState.LeasedUtcUntil < now)
                    .Select(mh => mh.MessageState)
                    .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}