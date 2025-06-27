using Mapster;
using Microsoft.EntityFrameworkCore;
using Neoledge.Nxc.Domain.Api.Federation;
using Neoledge.Nxc.Domain.Exceptions;
using Neoledge.NxC.Database;
using Neoledge.NxC.Database.Models;
using Neoledge.NxC.Repository.Interfaces;

namespace Neoledge.NxC.Repository.Imp
{
    public class FederationRepository(IAppDbContext context) : IFederationRepository
    {
        private Task<bool> ExistsAsync(string id, CancellationToken cancellationToken)
        {
            return context.Federations.AsNoTracking().AnyAsync(f => f.Id.ToLower() == id.ToLower(), cancellationToken);
        }

        public async Task<IList<Federation>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Federations.Include(f => f.Contacts).AsNoTracking().ToListAsync().ConfigureAwait(false);
        }

        public async Task<Federation> CreateAsync(CreateFederationParameters parameters, CancellationToken cancellationToken)
        {
            if (await ExistsAsync(parameters.Id, cancellationToken).ConfigureAwait(false))
                throw new EntityAlreadyExistsException(nameof(Federation), parameters.Id);
            var federation = parameters.Adapt<Federation>();
            await context.Federations.AddAsync(federation, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return federation;
        }

        public async Task<Federation> UpdateAsync(UpdateFederationParameters parameters, CancellationToken cancellationToken)
        {
            var federation = await context.Federations.FindAsync(parameters.Id, cancellationToken).ConfigureAwait(false)
                ?? throw new EntityNotFoundException(nameof(Federation), parameters.Id);

            if (!string.IsNullOrEmpty(parameters.Name))
                federation.Name = parameters.Name;

            if (!string.IsNullOrEmpty(parameters.Description))
                federation.Description = parameters.Description;

            if (parameters.Enabled != null)
                federation.Enabled = (bool)parameters.Enabled;

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return federation;
        }

        public async Task<Federation> AddContactAsync(AddFederationContactParameters parameters, CancellationToken cancellationToken)
        {
            var federation = await context.Federations.FindAsync(parameters.FederationId, cancellationToken).ConfigureAwait(false)
                ?? throw new EntityNotFoundException(nameof(Federation), parameters.FederationId);
            var federationContact = parameters.Adapt<FederationContact>();
            federation.Contacts.Add(federationContact);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return federation;
        }

        public async Task<FederationContact> UpdateContactAsync(UpdateFederationContactParameters parameters, CancellationToken cancellationToken)
        {
            var federationContact = await context.FederationContacts.FindAsync(parameters.Id, cancellationToken).ConfigureAwait(false)
                ?? throw new EntityNotFoundException(nameof(FederationContact), parameters.Id.ToString());

            if (!string.IsNullOrEmpty(parameters.FirstName))
                federationContact.FirstName = parameters.FirstName;

            if (!string.IsNullOrEmpty(parameters.LastName))
                federationContact.LastName = parameters.LastName;

            if (!string.IsNullOrEmpty(parameters.Email))
                federationContact.Email = parameters.Email;

            if (!string.IsNullOrEmpty(parameters.PhoneNumber))
                federationContact.PhoneNumber = parameters.PhoneNumber;

            if (parameters.Role != null && parameters.Role.HasValue)
                federationContact.Role = parameters.Role.Value;

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return federationContact;
        }

        public async Task DeleteContactAsync(string id, CancellationToken cancellationToken)
        {
            var federationContact = await context.FederationContacts.FindAsync(id, cancellationToken).ConfigureAwait(false)
                ?? throw new EntityNotFoundException(nameof(FederationContact), id.ToString());
            context.FederationContacts.Remove(federationContact);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task ExistAndActiveAsync(string federationId, CancellationToken cancellationToken)
        {
            var federation = await context.Federations.AsNoTracking()
                                                      .FirstOrDefaultAsync(f => f.Id.ToLower() == federationId.ToLower(), cancellationToken)
                                                      .ConfigureAwait(false) 
                                                      ?? throw new EntityNotFoundException(nameof(Federation), federationId);
            if (!federation.Enabled)
                throw new EntityValidationException("Federation is not active.");
        }
    }
}