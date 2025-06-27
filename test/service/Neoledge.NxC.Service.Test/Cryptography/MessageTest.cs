using Microsoft.Extensions.Options;
using Neoledge.NxC.Service.Cryptography.Extensions.Options;
using Neoledge.NxC.Service.Cryptography.Internal.Cipher;
using Neoledge.NxC.Service.Cryptography.Internal.Hash;
using Neoledge.NxC.Service.Cryptography.Internal.SessionKey;
using Neoledge.NxC.Service.Cryptography.Message;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Neoledge.NxC.Service.Test.Cryptography
{

    public class MessageTest
    {
        private static MessageManager GetMessageManager()
        {
            IOptions<CryptographyServiceOptions> options = Options.Create(new CryptographyServiceOptions());
            SymmetricCipherManager symmetricCipherManager = new();
            AsymmetricCipherManager asymmetricCipherManager = new(options);
            HashManager hashManager = new(options);
            SessionKeyManager sessionKeyManager = new(options);
            return new MessageManager(sessionKeyManager, symmetricCipherManager, hashManager, asymmetricCipherManager);
        }
        private static MemoryStream StreamFromString(string str)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            var ms = new MemoryStream(byteArray);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
        private static string StringFromStream(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using StreamReader reader = new(stream, Encoding.UTF8,leaveOpen:true);
            return reader.ReadToEnd();
        }

        [Fact]
        //Test basique de préparation d'un message 
        public async Task PrepareMessageAsync()
        {
            //Arrange
            var fromCert = X509CertificateLoader.LoadPkcs12FromFile("Cryptography/Asset/alice.elisedemo.com.pfx", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
            var toCert = X509CertificateLoader.LoadPkcs12FromFile("Cryptography/Asset/bob.elisedemo.com.pfx", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
            var messageManager = GetMessageManager();
            using var inBodyStream = StreamFromString("Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit...");
            using var outBodyStream = new MemoryStream();

            //Act
            var messageSceal = await messageManager.PrepareMessageAsync(fromCert, toCert, inBodyStream, outBodyStream, CancellationToken.None);

            //Assert
            Assert.NotNull(messageSceal);
            Assert.NotNull(messageSceal.EncryptedKey);
            Assert.NotNull(messageSceal.EncryptedIv);
            Assert.NotNull(messageSceal.SignedFileHash);
        }


        [Fact]
        //Tester la préparation et la vérification intermédiaire 
        public async Task CheckMessageAsync()
        {
            //Arrange
            var fromCert = X509CertificateLoader.LoadPkcs12FromFile("Cryptography/Asset/alice.elisedemo.com.pfx", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
            var toCert = X509CertificateLoader.LoadPkcs12FromFile("Cryptography/Asset/bob.elisedemo.com.pfx", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
            var messageManager = GetMessageManager();
            using var inBodyStream = StreamFromString("Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit...");
            using var outCryptedBodyStream = new MemoryStream();

            //Act
            //Préparer
            var messageSceal = await messageManager.PrepareMessageAsync(fromCert, toCert, inBodyStream, outCryptedBodyStream, CancellationToken.None);
            //Vérifier
            outCryptedBodyStream.Seek(0, SeekOrigin.Begin);
            var verificationResult = await messageManager.VerifyMessageAsync(fromCert, messageSceal, outCryptedBodyStream, CancellationToken.None);
            
            //Assert
            Assert.NotNull(verificationResult);
            Assert.Equal(VerificationResultCode.Ok, verificationResult.VerificationResultCode);
        }

        [Fact]
        //Tester la préparation, l'altération et la vérification intermédiaire 
        public async Task CheckAlteredMessageAsync()
        {
            //Arrange
            var fromCert = X509CertificateLoader.LoadPkcs12FromFile("Cryptography/Asset/alice.elisedemo.com.pfx", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
            var toCert = X509CertificateLoader.LoadPkcs12FromFile("Cryptography/Asset/bob.elisedemo.com.pfx", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
            var messageManager = GetMessageManager();
            using var inBodyStream = StreamFromString("Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit...");
            using var outCryptedBodyStream = new MemoryStream();

            //Act
            //Préparer
            var messageSceal = await messageManager.PrepareMessageAsync(fromCert, toCert, inBodyStream, outCryptedBodyStream, CancellationToken.None);
            //Corrompre
            outCryptedBodyStream.Seek(0, SeekOrigin.Begin);
            outCryptedBodyStream.WriteByte(26);
            outCryptedBodyStream.Seek(0, SeekOrigin.Begin);
            //Vérifier
            var verificationResult = await messageManager.VerifyMessageAsync(fromCert, messageSceal, outCryptedBodyStream, CancellationToken.None);

            //Assert
            Assert.NotNull(verificationResult);
            Assert.Equal(VerificationResultCode.FailedHashVerification, verificationResult.VerificationResultCode);
        }

        [Fact]
        //Test complet : la préparation par l'expéditeur, la vérification intermédiaire et la réception par le destinataire 
        public async Task CheckReceiveMessageAsync()
        {
            //Arrange
            var fromCert = X509CertificateLoader.LoadPkcs12FromFile("Cryptography/Asset/alice.elisedemo.com.pfx", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
            var toCert = X509CertificateLoader.LoadPkcs12FromFile("Cryptography/Asset/bob.elisedemo.com.pfx", "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);
            var messageManager = GetMessageManager();
            string messageContent = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit...";
            var inBodyStream = StreamFromString(messageContent);
            var outCryptedBodyStream = new MemoryStream(); 
            var outUncryptedBodyStream = new MemoryStream();

            //Act
            //Préparer
            var messageSceal = await messageManager.PrepareMessageAsync(fromCert, toCert, inBodyStream, outCryptedBodyStream, CancellationToken.None);
            //Vérifier
            outCryptedBodyStream.Seek(0, SeekOrigin.Begin);
            var verificationResult = await messageManager.VerifyMessageAsync(fromCert, messageSceal, outCryptedBodyStream, CancellationToken.None);
            //Réceptionner 
            outCryptedBodyStream.Seek(0, SeekOrigin.Begin);
            await messageManager.ReceiveMessageAsync(toCert, messageSceal, outCryptedBodyStream, outUncryptedBodyStream, CancellationToken.None);
            var decryptedBody = StringFromStream(outUncryptedBodyStream);

            //Assert
            Assert.NotNull(verificationResult);
            Assert.Equal(VerificationResultCode.Ok, verificationResult.VerificationResultCode);
            Assert.Equal(inBodyStream.Length, outUncryptedBodyStream.Length);
            Assert.Equal(messageContent, decryptedBody);
        }
    }
}
