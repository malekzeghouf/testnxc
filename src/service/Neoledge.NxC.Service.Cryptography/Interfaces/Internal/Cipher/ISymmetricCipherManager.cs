using System.Security.Cryptography;

namespace Neoledge.NxC.Service.Cryptography.Interfaces.Internal.Cipher
{
    public interface ISymmetricCipherManager
    {
        Task EncryptStreamAsync(SymmetricAlgorithm symmetricAlgorithm, Stream inStream, Stream outStream, CancellationToken cancellationToken);

        Task DecryptStreamAsync(SymmetricAlgorithm symmetricAlgorithm, Stream inStream, Stream outStream, CancellationToken cancellationToken);
    }
}