using Microsoft.Extensions.Options;
using Neoledge.NxC.Service.Certificate.Extensions.Options;
using Neoledge.NxC.Service.Certificate.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace Neoledge.NxC.Service.Test.Certificate
{
    public class CertificateSerializationTest
    {
        private static CertificateSerializationManager GetCertificateSerializationManager()
        {
            IOptions<CertificateServiceOptions> options = Options.Create(new CertificateServiceOptions());
            return new CertificateSerializationManager(options);
        }

        [Fact]
        public void TestExportCertificatePem()
        {
            //Arrange
            var aliceCert = X509CertificateLoader.LoadPkcs12FromFile("Cryptography/Asset/alice.elisedemo.com.pfx", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
            var certificateSerializationManager = GetCertificateSerializationManager();

            //Act
            var certificatePem = certificateSerializationManager.ExportCertificatePem(aliceCert);

            //Assert
            Assert.NotEmpty(certificatePem);
            Assert.StartsWith("-----BEGIN CERTIFICATE-----", certificatePem);
        }

        [Fact]
        public void TestExportPkcs8PrivateKeyPem()
        {
            //Arrange
            var aliceCert = X509CertificateLoader.LoadPkcs12FromFile("Cryptography/Asset/alice.elisedemo.com.pfx", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
            var certificateSerializationManager = GetCertificateSerializationManager();

            //Act
            var privateKeyPem = certificateSerializationManager.ExportPkcs8PrivateKeyPem(aliceCert, "p@assword");

            //Assert
            Assert.NotEmpty(privateKeyPem);
            Assert.StartsWith("-----BEGIN ENCRYPTED PRIVATE KEY-----", privateKeyPem);
        }

        [Fact]
        public void TestFullExportImport()
        {
            //Arrange
            var aliceCert = X509CertificateLoader.LoadPkcs12FromFile("Cryptography/Asset/alice.elisedemo.com.pfx", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
            var certificateSerializationManager = GetCertificateSerializationManager();
            var privateKeyPem = certificateSerializationManager.ExportPkcs8PrivateKeyPem(aliceCert, "p@assword");
            var certificatePem = certificateSerializationManager.ExportCertificatePem(aliceCert);

            //Act 
            var createdCert = certificateSerializationManager.CreateFromEncryptedPem(certificatePem, privateKeyPem, "p@assword");

            //Assert
            Assert.NotNull(createdCert);
            Assert.Equal(createdCert.Subject, aliceCert.Subject);
            Assert.Equal(createdCert.Thumbprint, aliceCert.Thumbprint);
            Assert.True(createdCert.HasPrivateKey);
        }
    }
}