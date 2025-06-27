using System.Security.Cryptography;

namespace Neoledge.NxC.Service.Cryptography.Interfaces.Internal.Cipher
{
    /// <summary>
    /// Gestion du chiffrement assymétrique
    /// </summary>
    public interface IAsymmetricCipherManager
    {
        /// <summary>
        /// Chiffrement
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] EncryptData(RSA rsa, ReadOnlySpan<byte> data);
        /// <summary>
        /// Déchiffrement 
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] DecryptData(RSA rsa, ReadOnlySpan<byte> data);
    }
}