namespace Neoledge.NxC.Service.Cryptography.Extensions.Options
{
    /// <summary>
    /// Valeurs par défaut
    /// </summary>
    public static class DefaultCryptographyServicesOptions
    {
        public const HashAlgorithmName DefaultHashAlgorithmName = HashAlgorithmName.SHA512;
        public const RSASignaturePadding DefaultRSASignaturePadding = RSASignaturePadding.Pkcs1;
        public const SymmetricAlgorithmName DefaultSymmetricAlgorithmName = SymmetricAlgorithmName.Aes;
        public const int DefaultSymmetricAlgorithmKeySize = 256;
        public const RSAEncryptionPadding DefaultRSAEncryptionPadding = RSAEncryptionPadding.Pkcs1;

        public static CryptographyServiceOptions Default => new CryptographyServiceOptions();
    }
}
