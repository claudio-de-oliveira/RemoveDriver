using IdentityModel.Client;

using Microsoft.AspNetCore.Builder;

using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using static IdentityModel.OidcConstants;

namespace AdministrativeApi.Test
{
    public class ClientMockApplication
    {
        protected readonly HttpClient _httpClient;
        protected readonly string _requestUri;
        public readonly string? TokenEndpoint;

        public ClientMockApplication(string uri, string? identityServerURI)
        {
            _requestUri = uri;

            _httpClient = new(new HttpClientHandler
            {
                // Bypass the SSH certificate
                ServerCertificateCustomValidationCallback =
                    (sender, cert, chain, sslPolicyErrors) => { return true; }
            })
            {
                BaseAddress = new Uri(_requestUri)
            };

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");

            var disco = _httpClient.GetDiscoveryDocumentAsync(identityServerURI).GetAwaiter().GetResult();
            
            if (string.IsNullOrEmpty(disco.Error))
            {
                this.TokenEndpoint = disco.TokenEndpoint;
                Console.WriteLine($"TokenEndpoint: {this.TokenEndpoint}");
            }
        }

        private static bool ServerCertificateCustomValidation(HttpRequestMessage requestMessage, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslErrors)
        {
            // It is possible to inspect the certificate provided by the server.
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine($"Requested URI: {requestMessage.RequestUri}");
            Console.WriteLine($"Effective date: {certificate.GetEffectiveDateString()}");
            Console.WriteLine($"Exp date: {certificate.GetExpirationDateString()}");
            Console.WriteLine($"Issuer: {certificate.Issuer}");
            Console.WriteLine($"Subject: {certificate.Subject}");

            // Based on the custom logic it is possible to decide whether the client considers certificate valid or not
            Console.WriteLine($"Errors: {sslErrors}\n");
            return sslErrors == SslPolicyErrors.None;
        }

        private static HttpClient CreateHttpClient(string fileName, string password)
        {
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback =
                (HttpRequestMessage message,
                    X509Certificate2? certificate,
                    X509Chain? chain,
                    SslPolicyErrors sslPolicyErrors) =>
                {
                    return ServerCertificateCustomValidation(message, certificate!, chain!, sslPolicyErrors);
                }
            };

            handler.ClientCertificates.Add(new X509Certificate2(fileName, password));

            return new HttpClient(handler);
        }

        public async Task<string> GetAccessToken(string clientId, string clientSecret)
        {
            var response = await _httpClient.RequestTokenAsync(new IdentityModel.Client.TokenRequest
            {
                Address = TokenEndpoint,
                GrantType = GrantTypes.ClientCredentials,
                ClientId = clientId,
                ClientSecret = clientSecret,
            });

            await Console.Out.WriteLineAsync(response.AccessToken);

            return response.AccessToken;
        }
    }
}
