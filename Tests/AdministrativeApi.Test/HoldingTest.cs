using AdministrativeApi.Test.Mock;

using Ardalis.GuardClauses;

using Azure.Core;

using System.Net;

namespace AdministrativeApi.Test
{
    public class HoldingTest
    {
        // private readonly ClientMockApplication ClientMockApplication;
        // 
        // public HoldingTest()
        // {
        //     // var folders = Directory.GetCurrentDirectory().Split('\\');
        //     // var pfxPath = "";
        //     // 
        //     // for (int i = 0; folders[i] != "Tests"; i++)
        //     //     pfxPath += $"{folders[i]}\\";
        //     // 
        //     // pfxPath += "AdministrativeApp\\certs\\administrativeclient_cert.pfx";
        //     // 
        //     // var password = "@Eaafe_301";
        // 
        // }

        [Fact]
        public async Task GET_Retorna_Todas_as_Holdings()
        {
            await using var application = new AdministrativeApiApplication(
                new Uri("https://localhost:7256")
                );

            var id = await HoldingMockData.CreateHolding(application);

            var url = $"/holding/get/id/{id}";

            string administrativeClientUrl = "https://localhost:7262";
            string identityServerUri = "https://localhost:7249";

            var clientMockApplication = new ClientMockApplication(
                $"{administrativeClientUrl}",
                identityServerUri
                );

            var client = application.CreateClient();

            var accessToken = await clientMockApplication.GetAccessToken("AdministrativeApp", "@Eaafe_301");

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            var result = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}