using Microsoft.Extensions.Options;
using Neoledge.NxC.Service.Cryptography.Extensions.Options;
using Neoledge.NxC.Service.Cryptography.Interfaces.Internal.Cipher;
using System.Security.Cryptography;

namespace Neoledge.NxC.Service.Cryptography.Internal.Cipher
{
    public class AsymmetricCipherManager(IOptions<CryptographyServiceOptions> cryptographyOptions) : IAsymmetricCipherManager
    {
        public byte[] DecryptData(RSA rsa, ReadOnlySpan<byte> data)
        {
            ArgumentNullException.ThrowIfNull(rsa);
            return rsa.Decrypt(data, cryptographyOptions.Value.RSAEncryptionPadding.Convert());
        }

        public byte[] EncryptData(RSA rsa, ReadOnlySpan<byte> data)
        {
            ArgumentNullException.ThrowIfNull(rsa);
            return rsa.Encrypt(data, cryptographyOptions.Value.RSAEncryptionPadding.Convert());
        }
    }
}