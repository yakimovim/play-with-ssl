using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace PlayWithSSL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var certificate = store.Certificates.OfType<X509Certificate2>()
                .First(c => c.FriendlyName == "Ivan Yakimov Test-only Certificate For Server Authorization");

            //var certificate = new X509Certificate2(
            //    @"certificateForServerAuthorization.pfx",
            //    "p@ssw0rd"
            //);

            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel(options =>
                        {
                            options.Listen(System.Net.IPAddress.Loopback, 44321, listenOptions =>
                            {
                                var connectionOptions = new HttpsConnectionAdapterOptions();
                                connectionOptions.ServerCertificate = certificate;

                                connectionOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                                connectionOptions.ClientCertificateValidation = (certificate, chain, errors) =>
                                {
                                    if (errors != SslPolicyErrors.None) return false;

                                    // Here is your code...

                                    return true;
                                };

                                listenOptions.UseHttps(connectionOptions);
                            });
                        })
                        .UseStartup<Startup>();
                });
        }
    }
}
