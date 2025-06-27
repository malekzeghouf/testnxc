namespace Neoledge.NxC.Service.Certificate.Serialization
{
    internal static class Converter
    {
        internal static System.Security.Cryptography.HashAlgorithmName Convert(this Extensions.Options.HashAlgorithmName hashAlgorithmName)
        {
            return hashAlgorithmName switch
            {
                Extensions.Options.HashAlgorithmName.MD5 => System.Security.Cryptography.HashAlgorithmName.MD5,
                Extensions.Options.HashAlgorithmName.SHA1 => System.Security.Cryptography.HashAlgorithmName.SHA1,
                Extensions.Options.HashAlgorithmName.SHA256 => System.Security.Cryptography.HashAlgorithmName.SHA256,
                Extensions.Options.HashAlgorithmName.SHA384 => System.Security.Cryptography.HashAlgorithmName.SHA384,
                Extensions.Options.HashAlgorithmName.SHA512 => System.Security.Cryptography.HashAlgorithmName.SHA512,
                Extensions.Options.HashAlgorithmName.SHA3_256 => System.Security.Cryptography.HashAlgorithmName.SHA3_256,
                Extensions.Options.HashAlgorithmName.SHA3_384 => System.Security.Cryptography.HashAlgorithmName.SHA3_384,
                Extensions.Options.HashAlgorithmName.SHA3_512 => System.Security.Cryptography.HashAlgorithmName.SHA3_512,
                _ => throw new NotImplementedException($"{nameof(Extensions.Options.HashAlgorithmName)} {hashAlgorithmName} not implemented"),
            };
        }
        internal static System.Security.Cryptography.PbeEncryptionAlgorithm Convert(this Extensions.Options.PbeEncryptionAlgorithm pbeEncryptionAlgorithm)
        {
            return pbeEncryptionAlgorithm switch
            {
                Extensions.Options.PbeEncryptionAlgorithm.Aes128Cbc => System.Security.Cryptography.PbeEncryptionAlgorithm.Aes128Cbc,
                Extensions.Options.PbeEncryptionAlgorithm.Aes192Cbc => System.Security.Cryptography.PbeEncryptionAlgorithm.Aes192Cbc,
                Extensions.Options.PbeEncryptionAlgorithm.Aes256Cbc => System.Security.Cryptography.PbeEncryptionAlgorithm.Aes256Cbc,
                Extensions.Options.PbeEncryptionAlgorithm.TripleDes3KeyPkcs12 => System.Security.Cryptography.PbeEncryptionAlgorithm.TripleDes3KeyPkcs12,
                _ => throw new NotImplementedException($"{nameof(Extensions.Options.PbeEncryptionAlgorithm)} {pbeEncryptionAlgorithm} not implemented"),
            };
        }
    }
}
