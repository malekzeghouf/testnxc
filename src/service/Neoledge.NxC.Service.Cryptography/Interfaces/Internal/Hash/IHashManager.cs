using System.Security.Cryptography;

namespace Neoledge.NxC.Service.Cryptography.Interfaces.Internal.Hash
{
    /// <summary>
    /// Gestion des opérations relatives aux hash
    /// </summary>
    public interface IHashManager
    {
        /// <summary>
        /// Calcul de l'empreinte d'un flux 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<byte[]> HashStreamAsync(Stream stream, CancellationToken cancellationToken);

        /// <summary>
        /// Signature d'une empreinte avec une clef RSA
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] SignHash(RSA rsa, byte[] data);

        /// <summary>
        /// Vérification de le signature d'une empreinte à partir d'une clef RSA 
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="hash"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        bool VerifyHash(RSA rsa, byte[] hash, byte[] signature);

    }
}
