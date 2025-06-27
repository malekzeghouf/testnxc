namespace Neoledge.NxC.Service.Certificate.Extensions.Options
{
    /// <summary>
    /// Options de fonctionnement des services de gestion des certificats
    /// </summary>
    public class CertificateServiceOptions
    {
        /// <summary>
        /// Nom de l’algorithme de hachage à utiliser avec la fonction de dérivation de clés (KDF) pour convertir un mot de passe en clé de chiffrement.
        /// </summary>
        public HashAlgorithmName PbeHashAlgorithmName { get; init; } = DefaultCertificateServiceOptions.DefaultPbeHashAlgorithmName;
        /// <summary>
        /// Algorithme à utiliser lors du chiffrement des données par mot de passe.
        /// </summary>
        public PbeEncryptionAlgorithm PbeEncryptionAlgorithm { get; init; } = DefaultCertificateServiceOptions.DefaultPbeEncryptionAlgorithm;
        /// <summary>
        /// Obtient le nombre d’itérations à fournir à la fonction de dérivation de clés (KDF) pour convertir un mot de passe en clé de chiffrement.
        /// </summary>
        public int PbeIterationCount { get; init; } = DefaultCertificateServiceOptions.DefaultPbeIterationCount;
        /// <summary>
        /// Taille minimum des mots de passe pour le chiffrement des données par mot de passe.
        /// </summary>
        public int PbeMinimumPasswordLength { get; init; } = DefaultCertificateServiceOptions.DefaultPbeMinimumPasswordLength;
    }
}