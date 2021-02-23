using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientDemo
{
    public static class Config
    {
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client 
                {
                    ClientId = "client",
                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes={ "api1" }
                }
            };
    }
}
