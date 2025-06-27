namespace Neoledge.NxC.Service.Cryptography.Internal
{
    internal static class Converters
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
        internal static System.Security.Cryptography.RSASignaturePadding Convert(this Extensions.Options.RSASignaturePadding rSASignaturePadding)
        {
            return rSASignaturePadding switch
            {
                Extensions.Options.RSASignaturePadding.Pkcs1 => System.Security.Cryptography.RSASignaturePadding.Pkcs1,
                Extensions.Options.RSASignaturePadding.Pss => System.Security.Cryptography.RSASignaturePadding.Pss,
                _ => throw new NotImplementedException($"{nameof(Extensions.Options.RSASignaturePadding)} {rSASignaturePadding} not implemented"),
            };
        }


        internal static System.Security.Cryptography.RSAEncryptionPadding Convert(this Extensions.Options.RSAEncryptionPadding rRSAEncryptionPadding)
        {
            return rRSAEncryptionPadding switch
            {
                Extensions.Options.RSAEncryptionPadding.Pkcs1 => System.Security.Cryptography.RSAEncryptionPadding.Pkcs1,
                Extensions.Options.RSAEncryptionPadding.OaepSHA1 => System.Security.Cryptography.RSAEncryptionPadding.OaepSHA1,
                Extensions.Options.RSAEncryptionPadding.OaepSHA256 => System.Security.Cryptography.RSAEncryptionPadding.OaepSHA256,
                Extensions.Options.RSAEncryptionPadding.OaepSHA384 => System.Security.Cryptography.RSAEncryptionPadding.OaepSHA384,
                Extensions.Options.RSAEncryptionPadding.OaepSHA512 => System.Security.Cryptography.RSAEncryptionPadding.OaepSHA512,
                Extensions.Options.RSAEncryptionPadding.OaepSHA3_256 => System.Security.Cryptography.RSAEncryptionPadding.OaepSHA3_256,
                Extensions.Options.RSAEncryptionPadding.OaepSHA3_384 => System.Security.Cryptography.RSAEncryptionPadding.OaepSHA3_384,
                Extensions.Options.RSAEncryptionPadding.OaepSHA3_512 => System.Security.Cryptography.RSAEncryptionPadding.OaepSHA3_512,
                _ => throw new NotImplementedException($"{nameof(Extensions.Options.RSAEncryptionPadding)} {rRSAEncryptionPadding} not implemented"),
            };
        }
    }
}
