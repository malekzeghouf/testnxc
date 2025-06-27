using Neoledge.Nxc.Domain.Database.Enum;
using Neoledge.NxC.Database.Models;

namespace Neoledge.NxC.Repository.Interfaces
{
    public interface IMessageRepository
    {
        Task SaveAsync(MessageHeader message, CancellationToken cancellationToken);

        Task<Guid?> GetLastMessageAsync(string memberId, string federationId, CancellationToken cancellationToken);

        Task<MessageHeader> GetMessageByIdAsync(Guid messageId, CancellationToken cancellationToken);
        Task InitMessageStateAsync(MessageState messageState, CancellationToken cancellationToken);

        Task UpdateMessageStateAsync(Guid messageId, MessageStatus status, CancellationToken cancellationToken);

        Task AddMessageHistoryAsync(MessageHistory messageHistory, CancellationToken cancellationToken);

        Task<List<MessageState>> GetExpiredLeasedMessagesAsync(DateTime now);
        Task SaveChangesAsync();
    }
}