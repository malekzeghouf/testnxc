namespace Neoledge.NxC.Service.Cryptography.Extensions.Options
{
    /// <summary>
    /// Options de fonctionnement des services cryptographiques
    /// </summary>
    public class CryptographyServiceOptions 
    {
        /// <summary>
        /// Specifies the name of a cryptographic hash algorithm.
        /// </summary>
        public HashAlgorithmName HashAlgorithmName { get; init; } = DefaultCryptographyServicesOptions.DefaultHashAlgorithmName;

        /// <summary>
        ///  Specifies the padding mode and parameters to use with RSA signature creation or verification operations.
        /// </summary>
        public RSASignaturePadding RSASignaturePadding { get; init; } = DefaultCryptographyServicesOptions.DefaultRSASignaturePadding;

        /// <summary>
        /// Specifies the padding mode and parameters to use with RSA encryption or decryption operations.
        /// </summary>
        public RSAEncryptionPadding RSAEncryptionPadding { get; init; } = DefaultCryptographyServicesOptions.DefaultRSAEncryptionPadding;

        /// <summary>
        /// Specifies the name of a symetric cypher algorithm.
        /// </summary>
        public SymmetricAlgorithmName SymmetricAlgorithmName { get; init; } = DefaultCryptographyServicesOptions.DefaultSymmetricAlgorithmName;

        // <summary>
        /// Specifies the size of symetric key.
        /// </summary>
        public int SymmetricAlgorithmKeySize { get; internal set; } = DefaultCryptographyServicesOptions.DefaultSymmetricAlgorithmKeySize;
    }
}