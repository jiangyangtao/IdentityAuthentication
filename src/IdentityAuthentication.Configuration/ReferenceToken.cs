using IdentityAuthentication.Model.Handles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Configuration
{
    public class ReferenceToken
    {
        public ReferenceToken() { }

        public ReferenceToken(AuthenticationResult result, DateTime expirationTime)
        {
            UserId = result.UserId;
            Username = result.Username;
            GrantSource = result.GrantSource;
            GrantType = result.GrantType;
            Client = result.Client;
            ExpirationTime = expirationTime;
        }

        public string UserId { set; get; }

        public string Username { set; get; }

        public string GrantSource { set; get; }

        public string GrantType { set; get; }

        public string Client { set; get; }

        public DateTime ExpirationTime { set; get; }

        public IReadOnlyDictionary<string, string> Metadata { set; get; }

        public IReadOnlyDictionary<string, string> ToReadOnlyDictionary()
        {
            var dic = new Dictionary<string, string>
            {
                {AuthenticationResult.UserIdPropertyName, UserId},
                {AuthenticationResult.UsernamePropertyName,Username},
                {AuthenticationResult.GrantSourcePropertyName,GrantType},
                {AuthenticationResult.GrantTypePropertyName,GrantSource},
                {AuthenticationResult.ClientPropertyName,Client},
                {IdentityAuthenticationDefaultKeys.Expiration,ExpirationTime.ToString()},
            };
            if (Metadata.IsNullOrEmpty()) return dic;

            foreach (var item in Metadata)
            {
                dic.Add(item.Key, item.Value);
            }

            return dic;
        }

        public AuthenticationResult GetAuthenticationResult()
        {
            return AuthenticationResult.CreateAuthenticationResult(
                userId: UserId,
                username: Username,
                grantSource: GrantSource,
                grantType: GrantType,
                client: Client,
                metadata: Metadata);
        }
    }
}
