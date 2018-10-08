using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Identity.API
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("post", "Post API") //resource 其实就是 scope，名称不同但是表示同一个意思，我们这里定义了一个scope 即 api1， scope 表示用户需要保护，且 client 想要 access 的资源
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    //注意，如下的 GrantType 代表了 IdentityServer 对该 Client 的配置中所允许该Client使用的授权模式是 Client Credential，也就是说这个 client 之后如果要访问这个 identity server，必须提供 client credential (ie. secret) 才能通过 Identity sever 的认证，而后 client 才能拿到它所想要的信息，比如 access token。
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "post" } //《-- 其实就是出现在 consent 屏幕上，问你是否同意把xxx给共享
                },
                new Client
                {
                    ClientId = "swagger",
                    ClientName = "swagger",
                    //注意，如下的 GrantType 代表了 IdentityServer 对该 Client 的配置中所允许该Client使用的授权模式是 Client Credential，也就是说这个 client 之后如果要访问这个 identity server，必须提供 client credential (ie. secret) 才能通过 Identity sever 的认证，而后 client 才能拿到它所想要的信息，比如 access token。
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = {"http://localhost:5001/swagger/oauth2-redirect.html"},
                    // secret for authentication
                    //ClientSecrets =
                    //{
                    //    new Secret("secret".Sha256())
                    //},

                    // scopes that client has access to
                    AllowedScopes = {"post" } //《-- 其实就是出现在 consent 屏幕上，问你是否同意把xxx给共享
                },
                //new Client
                //{
                //    ClientId = "swagger",
                //    ClientName = "swagger",
                //    AllowedGrantTypes  = GrantTypes.Implicit,
                //    AllowAccessTokensViaBrowser = true,
                //    RedirectUris = {"http://localhost:5001/swagger/oauth2-redirect.html"},

                //    AllowedScopes = {"post"}
                //}
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    Username = "charlie",
                    Password ="111111",
                    SubjectId ="1"
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }
}
