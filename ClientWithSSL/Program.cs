using System;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ClientWithSSL
{
    class Program
    {
        static async Task Main()
        {
            var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var certificate = store.Certificates.OfType<X509Certificate2>()
                .First(c => c.FriendlyName == "Ivan Yakimov Test-only Certificate For Client Authorization");

            //var certificate = new X509Certificate2(
            //    @"certificateForClientAuthorization.pfx",
            //    "p@ssw0rd"
            //);

            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (request, certificate, chain, errors) => {
                    if (errors != SslPolicyErrors.None) return false;

                    // Here is your code...

                    return true;
                }
            };

            handler.ClientCertificates.Add(certificate);

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:44321")
            };

            var result = await client.GetAsync("data");

            var content = await result.Content.ReadAsStringAsync();

            Console.WriteLine(content);

            Console.ReadLine();
        }
    }
}
