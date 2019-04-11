using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace X509Sample
{
    class Program
    {
        static void Main(string[] args)
        {
                byte[] data = Encoding.UTF8.GetBytes($"Hi bye {DateTimeOffset.Now}");
            string criteria = Environment.GetEnvironmentVariable("Cert") ?? "5094358121f2d3d7609f018ff2997e39d1eff7a9";
            //string subjectName = Environment.GetEnvironmentVariable("Cert") ?? "7847c72b58c2c2aaff9c9e6a68734fdbcd3ea57b";
            using (var s = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                s.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection cers = s.Certificates.Find(X509FindType.FindByThumbprint, criteria, false);
                X509Certificate2 cert = new X509Certificate2(cers[0]);

                var signature = Sign(data, cert);
                bool isValid = Verify(data, signature, cert);
                Console.WriteLine(isValid);
            }

            Console.ReadKey();
        }

        private static byte[] Sign(byte[] data, X509Certificate2 privateKey)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (privateKey == null)
            {
                throw new ArgumentNullException("privateKey");
            }
            if (!privateKey.HasPrivateKey)
            {
                throw new ArgumentException("invalid certificate", "privateKey");
            }


            var provider = privateKey.GetRSAPrivateKey();
            var bytes = provider.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return bytes;
        }

        private static bool Verify(byte[] data, byte[] signature, X509Certificate2 publicKey)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (publicKey == null)
            {
                throw new ArgumentNullException("privateKey");
            }
           


            var provider = publicKey.GetRSAPublicKey();
            var isOk = provider.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return isOk;
        }
    }
}
