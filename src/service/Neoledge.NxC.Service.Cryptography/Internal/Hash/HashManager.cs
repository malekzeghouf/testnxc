using Microsoft.Extensions.Options;
using Neoledge.NxC.Service.Cryptography.Extensions.Options;
using Neoledge.NxC.Service.Cryptography.Interfaces.Internal.Hash;
using System.Security.Cryptography;

namespace Neoledge.NxC.Service.Cryptography.Internal.Hash
{
    /// <summary>
    /// Gestion des opérations relatives aux hash
    /// </summary>
    /// <param name="cryptographyOptions"></param>
    public class HashManager(IOptions<CryptographyServiceOptions> cryptographyOptions) : IHashManager
    {
        public Task<byte[]> HashStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(stream);
            return HashStreamInternalAsync(stream, cancellationToken);
        }
        private async Task<byte[]> HashStreamInternalAsync(Stream stream, CancellationToken cancellationToken)
        {
            using HashAlgorithm algorithm = GetHashAlgorithm();
            return await algorithm.ComputeHashAsync(stream, cancellationToken).ConfigureAwait(false);
        }

        public byte[] SignHash(RSA rsa, byte[] data)
        {
            ArgumentNullException.ThrowIfNull(rsa);
            ArgumentNullException.ThrowIfNull(data);
            return rsa.SignHash(data, cryptographyOptions.Value.HashAlgorithmName.Convert(), cryptographyOptions.Value.RSASignaturePadding.Convert()); 
        }

        public bool VerifyHash(RSA rsa, byte[] hash, byte[] signature)
        {
            ArgumentNullException.ThrowIfNull(rsa);
            ArgumentNullException.ThrowIfNull(hash);
            ArgumentNullException.ThrowIfNull(signature);
            return rsa.VerifyHash(hash, signature, cryptographyOptions.Value.HashAlgorithmName.Convert(), cryptographyOptions.Value.RSASignaturePadding.Convert());
        }

        private HashAlgorithm GetHashAlgorithm()
        {
            if (cryptographyOptions.Value.HashAlgorithmName == Extensions.Options.HashAlgorithmName.SHA512)
                return SHA512.Create();
            if (cryptographyOptions.Value.HashAlgorithmName == Extensions.Options.HashAlgorithmName.SHA1) 
                return SHA1.Create();
            if (cryptographyOptions.Value.HashAlgorithmName == Extensions.Options.HashAlgorithmName.SHA256)
                return SHA256.Create();
            if (cryptographyOptions.Value.HashAlgorithmName == Extensions.Options.HashAlgorithmName.SHA384)
                return SHA384.Create();
            if (cryptographyOptions.Value.HashAlgorithmName == Extensions.Options.HashAlgorithmName.SHA3_256)
                return SHA3_256.Create();
            if (cryptographyOptions.Value.HashAlgorithmName == Extensions.Options.HashAlgorithmName.SHA3_384)
                return SHA3_384.Create();
            if (cryptographyOptions.Value.HashAlgorithmName == Extensions.Options.HashAlgorithmName.SHA3_512)
                return SHA3_512.Create();
            if (cryptographyOptions.Value.HashAlgorithmName == Extensions.Options.HashAlgorithmName.MD5)
                return MD5.Create();
            throw new NotImplementedException($"{nameof(Extensions.Options.HashAlgorithmName)} {cryptographyOptions.Value.HashAlgorithmName} not implemented");
        }
    }
}
