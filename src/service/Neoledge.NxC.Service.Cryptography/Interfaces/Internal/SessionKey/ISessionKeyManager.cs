using System.Security.Cryptography;

namespace Neoledge.NxC.Service.Cryptography.Interfaces.Internal.SessionKey
{
    public interface ISessionKeyManager
    {
        /// <summary>
        /// Générère une nouvelle clef de session de chiffrement symétrique
        /// </summary>
        /// <returns></returns>
        public SymmetricAlgorithm NewSessionKey();

        /// <summary>
        /// Créer une clef à partir de ses composants
        /// </summary>
        /// <param name="key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        public SymmetricAlgorithm Create(byte[] key, byte[] IV);
    }
}