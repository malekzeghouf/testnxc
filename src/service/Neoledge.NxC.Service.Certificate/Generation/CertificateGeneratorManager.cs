using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Neoledge.NxC.Service.Certificate.Generation
{
    public class CertificateGeneratorManager : ICertificateGeneratorManager
    {
        public X509Certificate2 GenerateSelfSignedCertificate(string subjectName, int validYears = 1)
        {
            // Create RSA key pair
            using var rsa = RSA.Create(4096);  // 4096-bit key for strong encryption

            // Create certificate request
            var request = new CertificateRequest(
                new X500DistinguishedName($"CN={subjectName}"),
                rsa,
                HashAlgorithmName.SHA512,
                RSASignaturePadding.Pkcs1);

            // Set basic constraints
            request.CertificateExtensions.Add(
                new X509BasicConstraintsExtension(false, false, 0, false));

            // Set key usage for encryption
            request.CertificateExtensions.Add(
                new X509KeyUsageExtension(
                    X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DataEncipherment,
                    true));

            // Create self-signed certificate
            var certificate = request.CreateSelfSigned(
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddYears(validYears));

            var certWithKey = X509CertificateLoader.LoadPkcs12(certificate.Export(X509ContentType.Pkcs12), null, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

            return certWithKey;
        }
    }
}