namespace Neoledge.NxC.Service.Certificate.Extensions.Options
{
    /// <summary>
    /// Spécifie les algorithmes de chiffrement à utiliser avec le chiffrement par mot de passe.
    /// </summary>
    public enum PbeEncryptionAlgorithm
    {
        /// <summary>
        /// Indique que le chiffrement doit être effectué avec l’algorithme AES-128 en mode CBC avec remplissage PKCS#7.
        /// </summary>
        Aes128Cbc,
        /// <summary>
        /// Indique que le chiffrement doit être effectué avec l’algorithme AES-192 en mode CBC avec remplissage PKCS#7.
        /// </summary>
        Aes192Cbc,
        /// <summary>
        /// Indique que le chiffrement doit être effectué avec l’algorithme AES-256 en mode CBC avec remplissage PKCS#7.
        /// </summary>
        Aes256Cbc,
        /// <summary>
        /// Indique que le chiffrement doit être effectué avec l’algorithme TripleDES en mode CBC avec une clé 192 bits dérivée à l’aide de la fonction de dérivation de clés (KDF) de PKCS#12.
        /// </summary>
        TripleDes3KeyPkcs12,

    }
}
