using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.NxC.Service.MinoStorage.Interfaces
{
    public interface IStorageService
    {
        Task<Guid> UploadFileToPublicBucketAsync(Stream stream, string contentType , CancellationToken cancellationToken);
       
        Task<string> TransferFileToInboxAsync(Guid fileId, string federationId, string recipientMemberId, string recipientInboxId, string messageId, CancellationToken cancellationToken);

        Task GetFileFromPublicAsync(Guid fileId, Stream file, CancellationToken cancellationToken);
       
        Task<bool> GetBodyFromBucketAsync(Stream file, string federationId, string memberId, string inboxId,string messageId, CancellationToken cancellationToken);
    }
}
