using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace WinterSpine.IdentityServer.Configs
{
    public class Config
    {
        internal static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("role", "Role", new List<string>{ "role" })
            };
        }

        internal static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = "WinterSpine.IdentityServer.Web",
                    ClientName = "WinterSpine IdentityServer Web App",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = {  "http://localhost:10000/signin-oidc" },
                    PostLogoutRedirectUris ={ "http://localhost:10000/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        JwtClaimTypes.Role
                    }
                },
                new Client()
                {
                    ClientId = "WinterSpine.IdentityServer.MvcClient",
                    ClientName = "WinterSpine IdentityServer Mvc Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris = {  "http://localhost:10001/signin-oidc" },
                    PostLogoutRedirectUris ={ "http://localhost:10001/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        StandardScopes.Email,
                        JwtClaimTypes.Role
                    }
                }
            };
        }

        internal static List<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser()
                {
                    SubjectId = Guid.Empty.ToString(),
                    Username = "admin",
                    Password = "P@ssw0rd!",
                    Claims = new List<Claim>()
                    {
                        new Claim(JwtClaimTypes.Role, "Admin"),
                        new Claim(JwtClaimTypes.Name, "Jaliya Udagedara"),
                        new Claim(JwtClaimTypes.GivenName, "Jaliya"),
                        new Claim(JwtClaimTypes.FamilyName, "Udagedara"),
                        new Claim(JwtClaimTypes.Email, "jaliya.udagedara@gmail.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", System.Security.Claims.ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://jaliyaudagedara.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'WinterSpine Way', 'locality': 'English', 'postal_code': 55555, 'country': 'United States' }", IdentityServerConstants.ClaimValueTypes.Json)
                    }
                },
                new TestUser()
                {
                    SubjectId = new Guid("11111111-1111-1111-1111-111111111111").ToString(),
                    Username = "demo",
                    Password = "Welcome@123",
                    Claims = new List<Claim>()
                    {
                        new Claim(JwtClaimTypes.Role, "User"),
                        new Claim(JwtClaimTypes.Name, "John Doe"),
                        new Claim(JwtClaimTypes.GivenName, "John"),
                        new Claim(JwtClaimTypes.FamilyName, "Doe"),
                        new Claim(JwtClaimTypes.Email, "john@doe.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", System.Security.Claims.ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://johndoe.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'WinterSpine Way', 'locality': 'English', 'postal_code': 55555, 'country': 'United States' }", IdentityServerConstants.ClaimValueTypes.Json)
                    }
                }
            };
        }
    }
}
