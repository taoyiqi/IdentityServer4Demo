using System;
using System.Net.Http;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace ClientPasswordDemo
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var httpClient = new HttpClient();

            var requestUrl = "http://localhost:5001/identity";
            //requestUrl = "http://www.baidu.com";

            var response = await httpClient.GetAsync(requestUrl);

            Console.WriteLine(response.StatusCode);

            httpClient = new HttpClient();

            var disco = await httpClient.GetDiscoveryDocumentAsync("http://localhost:5000");

            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var tokenResponse = await httpClient.RequestPasswordTokenAsync(
                new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "password.client",
                    ClientSecret = "secret",
                    Scope = "api2",
                    UserName = "bob",
                    Password = "password"
                });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.AccessToken);

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var resp = await client.GetAsync(requestUrl);

            if (!resp.IsSuccessStatusCode)
            {
                Console.WriteLine(resp.StatusCode);
                return;
            }

            var content = await resp.Content.ReadAsStringAsync();
            Console.WriteLine(JArray.Parse(content));

            Console.WriteLine("END");
        }
    }
}
