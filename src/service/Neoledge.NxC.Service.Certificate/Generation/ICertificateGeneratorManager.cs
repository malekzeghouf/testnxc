using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Neoledge.NxC.Service.Certificate.Generation
{
    public interface ICertificateGeneratorManager
    {
        X509Certificate2 GenerateSelfSignedCertificate(string subjectName, int validYears = 5);
    }
}
