using Neoledge.NxC.Service.Cryptography.Interfaces.Internal.Cipher;
using System.IO;
using System.Security.Cryptography;

namespace Neoledge.NxC.Service.Cryptography.Internal.Cipher
{
    /// <summary>
    /// Chiffrement symétrique de flux 
    /// </summary>
    public class SymmetricCipherManager : ISymmetricCipherManager
    {
        public Task DecryptStreamAsync(SymmetricAlgorithm symmetricAlgorithm, Stream inStream, Stream outStream, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(symmetricAlgorithm);
            ArgumentNullException.ThrowIfNull(inStream);
            ArgumentNullException.ThrowIfNull(outStream);
            return DecryptStreamInternalAsync(symmetricAlgorithm, inStream, outStream, cancellationToken);
        }

        private static async Task DecryptStreamInternalAsync(SymmetricAlgorithm symmetricAlgorithm, Stream inStream, Stream outStream, CancellationToken cancellationToken)
        {
            using var decryptor = symmetricAlgorithm.CreateDecryptor();
            using CryptoStream cryptoStream = new(inStream, decryptor, CryptoStreamMode.Read,true);
            await cryptoStream.CopyToAsync(outStream, cancellationToken).ConfigureAwait(false);
        }

        public Task EncryptStreamAsync(SymmetricAlgorithm symmetricAlgorithm, Stream inStream, Stream outStream, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(symmetricAlgorithm);
            ArgumentNullException.ThrowIfNull(inStream);
            ArgumentNullException.ThrowIfNull(outStream);
            return EncryptStreamInternalAsync(symmetricAlgorithm, inStream, outStream, cancellationToken);
        }

        private static async Task EncryptStreamInternalAsync(SymmetricAlgorithm symmetricAlgorithm, Stream inStream, Stream outStream, CancellationToken cancellationToken)
        {
            using var encryptor = symmetricAlgorithm.CreateEncryptor();
            using CryptoStream cryptoStream = new(outStream, encryptor, CryptoStreamMode.Write,true);
            await inStream.CopyToAsync(cryptoStream, cancellationToken).ConfigureAwait(false);
        }
    }
}
