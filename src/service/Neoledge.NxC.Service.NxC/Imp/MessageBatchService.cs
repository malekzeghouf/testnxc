using Neoledge.Nxc.Domain.Database.Enum;
using Neoledge.NxC.Repository.Interfaces;
using Neoledge.NxC.Service.NxC.Interfaces;


namespace Neoledge.NxC.Service.NxC.Imp
{
    public class MessageBatchService (IMessageRepository messageRepository): IMessageBatchService
    {
        public async Task UpdateMsgStateAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var expiredMessages = await messageRepository.GetExpiredLeasedMessagesAsync(now);

            foreach (var state in expiredMessages)
            {
                state.Status = MessageStatus.Pending;
                state.LeasedUtcAt = null;
                state.LeasedUtcUntil = null;
            }

            await messageRepository.SaveChangesAsync();
        }
    }
}
