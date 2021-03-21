using System;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace api.testing.Extensions
{
    public static class AuthorizationExtensions
    {
        public static bool IsAuthorized(this HttpRequest request)
        {
            if (!request.Headers.ContainsKey("Authorization")) return false;

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                var password = credentials[1];

                return $"{username}:{password}" == "test:newyork1";
            }
            catch { /*nothing*/ }

            return false;
        }
    }
}
