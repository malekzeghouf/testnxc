using Minio;
using Minio.DataModel.Args;
using Neoledge.NxC.Service.MinoStorage.Interfaces;

namespace Neoledge.NxC.Service.MinoStorage.Imp
{
    public class StorageService(IMinioClient minioClient) : IStorageService
    {
        private readonly string PublicBucketName = "public";

        public async Task<Guid> UploadFileToPublicBucketAsync(Stream stream, string contentType, CancellationToken cancellationToken)
        {
            await CreateBucketIfNotExistAsync(PublicBucketName, cancellationToken).ConfigureAwait(false);

            var uniqueFileName = Guid.NewGuid();

            // TODO: Check if we need to encrypt object .WithServerSideEncryption(ssec) https://min.io/docs/minio/linux/developers/dotnet/API.html#putobjectasync-putobjectargs-args
            var res = await minioClient.PutObjectAsync(new PutObjectArgs()
                           .WithBucket(PublicBucketName)
                           .WithObject(uniqueFileName.ToString())
                           .WithStreamData(stream)
                           .WithObjectSize(stream.Length)
                           .WithContentType(contentType), cancellationToken
                           ).ConfigureAwait(false);
            return uniqueFileName;
        }

        public async Task<string> TransferFileToInboxAsync(Guid fileId, string federationId, string memberId, string inboxId, string messageId, CancellationToken cancellationToken)
        {
            // TODO: Proposition make buket name federation/MemberId as sufiix name

            string destinationObject = $"{memberId}/{inboxId}/{messageId}";

            var stat = await minioClient.StatObjectAsync(new StatObjectArgs()
                                       .WithBucket(PublicBucketName)
                                       .WithObject(fileId.ToString()));

            await CreateBucketIfNotExistAsync(federationId, cancellationToken).ConfigureAwait(false);

            // Copy the object
            await minioClient.CopyObjectAsync(
                new CopyObjectArgs()
                .WithBucket(federationId)
                .WithObject(destinationObject)
                .WithCopyObjectSource(new CopySourceObjectArgs().WithBucket(PublicBucketName).WithObject(fileId.ToString()))
                , cancellationToken
                ).ConfigureAwait(false);

            // Delete the source object (to achieve CUT instead of COPY)
            await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(PublicBucketName)
                .WithObject(fileId.ToString()), cancellationToken
                ).ConfigureAwait(false);

            return destinationObject;
        }

        public async Task GetFileFromPublicAsync(Guid fileId, Stream file, CancellationToken cancellationToken)
        {
            await minioClient.GetObjectAsync(new GetObjectArgs()
                           .WithBucket(PublicBucketName)
                           .WithObject(fileId.ToString())
                           .WithCallbackStream(stream =>
                           {
                               stream.CopyTo(file);
                           }), cancellationToken).ConfigureAwait(false);
            file.Seek(0, SeekOrigin.Begin);
        }

        public Task<bool> GetBodyFromBucketAsync(Stream file, string federationId, string memberId, string inboxId, string messageId, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(federationId);
            ArgumentNullException.ThrowIfNullOrEmpty(memberId);
            ArgumentNullException.ThrowIfNullOrEmpty(inboxId);
            ArgumentNullException.ThrowIfNullOrEmpty(messageId);
            return GetBodyFromBucketInternalAsync(file, federationId, memberId, inboxId, messageId, cancellationToken);
        }

        private async Task<bool> GetBodyFromBucketInternalAsync(Stream file, string federationId, string memberId, string inboxId, string messageId, CancellationToken cancellationToken)
        {
            var objectName = $"{memberId}/{inboxId}/{messageId}";

            var getObjectArgs = new GetObjectArgs()
                .WithBucket(federationId)
                .WithObject(objectName)
                .WithCallbackStream((stream) =>
                {
                    stream.CopyTo(file);
                });

            // Exécuter la requête de téléchargement
            await minioClient.GetObjectAsync(getObjectArgs, cancellationToken).ConfigureAwait(false);

            file.Position = 0;
            return true;
        }

        private Task<bool> BucketExistsAsync(string bucketName, CancellationToken cancellationToken)
        {
            return minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName), cancellationToken);
        }

        private async Task CreateBucketIfNotExistAsync(string bucketName, CancellationToken cancellationToken)
        {
            if (!await BucketExistsAsync(bucketName, cancellationToken).ConfigureAwait(false))
            {
                await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName), cancellationToken).ConfigureAwait(false);
            }
        }
    }
}