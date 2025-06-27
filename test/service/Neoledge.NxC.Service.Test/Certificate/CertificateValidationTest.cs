using Microsoft.Extensions.Logging;
using Moq;
using Neoledge.NxC.Service.Certificate.Validation;
using System.Security.Cryptography.X509Certificates;

namespace Neoledge.NxC.Service.Test.Certificate
{
    public class CertificateValidationTest
    {
        private static CertificateValidationManager GetCertificateValidationManager()
        {
            return new CertificateValidationManager(Mock.Of<ILogger<CertificateValidationManager>>());
        }
        [Fact]
        public void TestDisableAllValidationPolicy()
        {
            //Arrange
            var certificateManager = GetCertificateValidationManager();
            var policy = new ValidationPolicy { PolicyIdentifier="T",DisableAllValidation=true };
            var aliceCert = X509CertificateLoader.LoadPkcs12FromFile("Cryptography/Asset/alice.elisedemo.com.pfx", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);


            //Act
            var r = certificateManager.Validate(policy, aliceCert);

            //Assert
            Assert.NotNull(r);
            Assert.True(r.Validated);
            Assert.Equal(r.PolicyIdentifier, policy.PolicyIdentifier);
        }

        [Fact]
        public void TestAutoSignedCertShouldFail()
        {
            //Arrange
            var certificateManager = GetCertificateValidationManager();
            var policy = new ValidationPolicy { PolicyIdentifier = "T", DisableAllValidation = false };
            var aliceCert = X509CertificateLoader.LoadPkcs12FromFile("Cryptography/Asset/alice.elisedemo.com.pfx", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);


            //Act
            var r = certificateManager.Validate(policy, aliceCert);

            //Assert
            Assert.NotNull(r);
            Assert.False(r.Validated);
            Assert.Equal(r.PolicyIdentifier, policy.PolicyIdentifier);
        }

        [Fact]
        public void TestAutoSignedWithCustomRootTrust()
        {
            //Arrange
            var certificateManager = GetCertificateValidationManager();
            var policy = new ValidationPolicy { PolicyIdentifier = "T", DisableAllValidation = false ,ChainTrustMode = ChainTrustMode.CustomRootTrust};
            var aliceCert = X509CertificateLoader.LoadPkcs12FromFile("Cryptography/Asset/alice.elisedemo.com.pfx", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
            policy.CustomTrustStore.Add(aliceCert);

            //Act
            var r = certificateManager.Validate(policy, aliceCert);

            //Assert
            Assert.NotNull(r);
            Assert.True(r.Validated);
            Assert.Equal(r.PolicyIdentifier, policy.PolicyIdentifier);
        }
    }
}
