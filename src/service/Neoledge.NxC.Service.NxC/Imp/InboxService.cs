using Mapster;
using Neoledge.Nxc.Domain.Api.Inbox;
using Neoledge.NxC.Repository.Interfaces;
using Neoledge.NxC.Service.NxC.Interfaces;

namespace Neoledge.NxC.Service.NxC.Imp
{
    internal class InboxService(
        IInboxRepository inboxRepository,
        IFederationRepository federationRepository,
        IMemberRepository memberRepository) : IInboxService
    {
        public Task<IList<InboxResponse>> GetMyInboxesAsync(string federationId, string memberId, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(federationId);
            ArgumentException.ThrowIfNullOrEmpty(memberId);
            return GetMyInboxesInternalAsync(federationId, memberId, cancellationToken);
        }

        private async Task<IList<InboxResponse>> GetMyInboxesInternalAsync(string federationId, string memberId, CancellationToken cancellationToken)
        {
            // TODO: maybe execute one check federation active and member active in one function
            await federationRepository.ExistAndActiveAsync(federationId, cancellationToken).ConfigureAwait(false);
            await memberRepository.ExistAndActiveAndBelongToFederationAsync(federationId, memberId, cancellationToken).ConfigureAwait(false);

            var inboxeses = await inboxRepository.GetInboxesAsync(memberId, cancellationToken).ConfigureAwait(false);

            return inboxeses.Adapt<IList<InboxResponse>>();
        }

        public Task<IList<InboxResponse>> GetInboxesForMemberAsync(string federationId, string memberId, string forMemberId, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(federationId);
            ArgumentException.ThrowIfNullOrEmpty(memberId);
            return GetInboxesForMemberInternalAsync(federationId, memberId, forMemberId, cancellationToken);
        }

        private async Task<IList<InboxResponse>> GetInboxesForMemberInternalAsync(string federationId, string memberId, string forMemberId, CancellationToken cancellationToken)
        {
            // TODO: maybe execute one check federation active and member active in one function
            await federationRepository.ExistAndActiveAsync(federationId, cancellationToken).ConfigureAwait(false);
            await memberRepository.ExistAndActiveAndBelongToFederationAsync(federationId, memberId, cancellationToken).ConfigureAwait(false);

            var inboxeses = await inboxRepository.GetInboxesForMemberAsync(memberId, forMemberId, cancellationToken).ConfigureAwait(false);

            return inboxeses.Adapt<IList<InboxResponse>>();
        }

        public Task<string> GetInboxPublicCertificateAsync(string federationId, string inboxId, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(federationId);
            ArgumentException.ThrowIfNullOrEmpty(inboxId);
            return GetInboxPublicCertificateInternalAsync(federationId, inboxId, cancellationToken);
        }

        private async Task<string> GetInboxPublicCertificateInternalAsync(string federationId, string inboxId, CancellationToken cancellationToken)
        {
            await federationRepository.ExistAndActiveAsync(federationId, cancellationToken).ConfigureAwait(false);
            return await inboxRepository.GetInboxPublicCertificateAsync(inboxId, cancellationToken).ConfigureAwait(false);
        }

        public Task<bool> PublishPublicCertificateAsync(string federationId, string memberId, string inboxId, string publicCert, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(federationId);
            ArgumentException.ThrowIfNullOrEmpty(memberId);
            ArgumentException.ThrowIfNullOrEmpty(inboxId);
            ArgumentException.ThrowIfNullOrEmpty(publicCert);
            // TODO : check publicCert is valid PEM format and not empty, maybe using
            // certificateValidationManager.Validate
            return PublishPublicCertificateInternalAsync(federationId, memberId, inboxId, publicCert, cancellationToken);
        }

        private async Task<bool> PublishPublicCertificateInternalAsync(string federationId, string memberId, string inboxId, string publicCert, CancellationToken cancellationToken)
        {
            await federationRepository.ExistAndActiveAsync(federationId, cancellationToken).ConfigureAwait(false);
            await memberRepository.ExistAndActiveAndBelongToFederationAsync(federationId, memberId, cancellationToken).ConfigureAwait(false);
            return await inboxRepository.AddInboxPublicCertificateAsync(inboxId, publicCert, cancellationToken).ConfigureAwait(false);
        }
    }
}