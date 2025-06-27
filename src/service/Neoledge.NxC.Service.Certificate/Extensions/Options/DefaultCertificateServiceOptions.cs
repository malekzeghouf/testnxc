namespace Neoledge.NxC.Service.Certificate.Extensions.Options
{
    /// <summary>
    /// Paramètres par défaut
    /// </summary>
    //https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html#pbkdf2
    public static class DefaultCertificateServiceOptions
    {
        public const HashAlgorithmName DefaultPbeHashAlgorithmName = HashAlgorithmName.SHA512;
        public const PbeEncryptionAlgorithm DefaultPbeEncryptionAlgorithm = PbeEncryptionAlgorithm.Aes256Cbc;
        public const int DefaultPbeIterationCount = 210000;
        public const int DefaultPbeMinimumPasswordLength = 8;
    }
}
