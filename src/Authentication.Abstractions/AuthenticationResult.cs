using IdentityAuthentication.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Abstractions
{
    public sealed class AuthenticationResult
    {
        private AuthenticationResult(string userId, string authenticationSource, IReadOnlyDictionary<string, string> metadata)
        {
            UserId = userId;
            AuthenticationSource = authenticationSource;
            Metadata = metadata;
        }

        public string UserId { get; }

        public string AuthenticationSource { get; }

        public IReadOnlyDictionary<string, string> Metadata { get; }

        public static AuthenticationResult CreateAuthenticationResult(string userId, string authenticationSource, IReadOnlyDictionary<string, string> metadata)
            => new(userId, authenticationSource, metadata);

        public JObject ToJObject()
        {
            var result = new JObject {
                new JProperty(nameof(UserId),UserId),
                new JProperty(nameof(AuthenticationSource),AuthenticationSource),
            };

            if (Metadata.NotNullAndEmpty())
            {
                foreach (var item in Metadata)
                {
                    result.Add(item.Key, item.Value);
                }
            }

            return result;
        }
    }
}
