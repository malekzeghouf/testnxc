using Microsoft.Extensions.Options;
using Neoledge.NxC.Service.Cryptography.Extensions.Options;
using Neoledge.NxC.Service.Cryptography.Interfaces.Internal.SessionKey;
using System.Security.Cryptography;

namespace Neoledge.NxC.Service.Cryptography.Internal.SessionKey
{
    public class SessionKeyManager(IOptions<CryptographyServiceOptions> cryptographyOptions) : ISessionKeyManager
    {
        /// <summary>
        /// Création d'une clef à partir de ses composantes
        /// </summary>
        /// <param name="key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public SymmetricAlgorithm Create(byte[] key, byte[] IV)
        {
            var symmetricAlgorithm = GetSymmetricAlgorithm();
            symmetricAlgorithm.Key = key;
            symmetricAlgorithm.IV = IV;
            return symmetricAlgorithm;
        }

        /// <summary>
        /// Génération d'une nouvelle clef pour une session de chiffrement
        /// </summary>
        /// <returns></returns>
        public SymmetricAlgorithm NewSessionKey()
        {
            var symmetricAlgorithm = GetSymmetricAlgorithm();
            symmetricAlgorithm.KeySize = cryptographyOptions.Value.SymmetricAlgorithmKeySize;
            symmetricAlgorithm.GenerateIV();
            symmetricAlgorithm.GenerateKey();
            return symmetricAlgorithm;
        }

        private SymmetricAlgorithm GetSymmetricAlgorithm()
        {
            return cryptographyOptions.Value.SymmetricAlgorithmName switch
            {
                Extensions.Options.SymmetricAlgorithmName.TripleDES =>  TripleDES.Create(),
                Extensions.Options.SymmetricAlgorithmName.DES =>  DES.Create(),
                Extensions.Options.SymmetricAlgorithmName.RC2 =>  RC2.Create(),
                Extensions.Options.SymmetricAlgorithmName.Aes =>  Aes.Create(),
                _ => throw new NotImplementedException($"{nameof(cryptographyOptions.Value.SymmetricAlgorithmName)} {cryptographyOptions.Value.SymmetricAlgorithmName} not implemented"),
            };
        }
    }
}