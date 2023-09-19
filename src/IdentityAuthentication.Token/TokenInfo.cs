﻿using IdentityAuthentication.Configuration.Model;
using IdentityAuthentication.Extensions;
using IdentityAuthentication.Model.Extensions;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IdentityAuthentication.Token
{
    internal class TokenInfo
    {
        public TokenInfo()
        {
        }

        public TokenInfo(AuthenticationResult result)
        {
            UserId = result.UserId;
            Username = result.Username;
            GrantSource = result.GrantSource;
            GrantType = result.GrantType;
            Client = result.Client;
            Metadata = result.Metadata;
        }

        public string UserId { set; get; }

        public string Username { set; get; }

        public string GrantSource { set; get; }

        public string GrantType { set; get; }

        public string Client { set; get; }

        public DateTime NotBefore { set; get; } = DateTime.Now;

        public DateTime ExpirationTime { set; get; }

        public DateTime IssueTime { set; get; } = DateTime.Now;

        public IReadOnlyDictionary<string, string> Metadata { set; get; }

        #region static property

        public static string UserIdKey = nameof(UserId);

        public static string UsernameKey = nameof(Username);

        public static string GrantSourceKey = nameof(GrantSource);

        public static string GrantTypeKey = nameof(GrantType);

        public static string ClientKey = nameof(Client);

        public static string NotBeforeKey = nameof(NotBefore);

        public static string IssueTimeKey = nameof(IssueTime);

        public static string ExpirationKey = ClaimTypes.Expiration;

        public static string[] TokenPropertyKeys = new string[]
        {
            UserIdKey ,
            UsernameKey,
            GrantSourceKey,
            GrantTypeKey,
            ClientKey,
            NotBeforeKey,
            IssueTimeKey,
            ExpirationKey
        };

        #endregion

        public override string ToString() => JsonConvert.SerializeObject(this);

        public Claim[] BuildClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(UserIdKey, UserId),
                new Claim(UsernameKey,Username),
                new Claim(GrantSourceKey,GrantSource),
                new Claim(GrantTypeKey,GrantType),
                new Claim(ClientKey,Client),
                new Claim(NotBeforeKey,NotBefore.ToString()),
                new Claim(IssueTimeKey,IssueTime.ToString()),
                new Claim(ExpirationKey,ExpirationTime.ToString()),
            };
            if (Metadata.IsNullOrEmpty()) return claims.ToArray();

            foreach (var item in Metadata)
            {
                claims.Add(new Claim(item.Key, item.Value));
            }

            return claims.ToArray();
        }

        public static bool TryParse(string json, out TokenInfo? accessToken)
        {
            accessToken = null;

            if (json.IsNullOrEmpty()) return false;

            var token = JsonConvert.DeserializeObject<TokenInfo>(json);
            if (token == null) return false;

            accessToken = token;
            return true;
        }

        public AuthenticationResult BuildAuthenticationResult()
        {
            return AuthenticationResult.CreateAuthenticationResult(UserId, Username, GrantSource, GrantType, Client, Metadata);
        }

        public static TokenInfo CreateToken(AuthenticationResult result) => new(result);

        public static bool TryParse(IEnumerable<Claim> claims, out TokenInfo? tokenInfo)
        {
            tokenInfo = null;
            if (claims.IsNullOrEmpty()) return false;


            var userIdClaim = claims.FirstOrDefault(a => a.Type == UserIdKey);
            if (userIdClaim == null) return false;
            if (userIdClaim.Value.IsNullOrEmpty()) return false;


            var usernameClaim = claims.FirstOrDefault(a => a.Type == UsernameKey);
            if (usernameClaim == null) return false;
            if (usernameClaim.Value.IsNullOrEmpty()) return false;


            var grantSourceClaim = claims.FirstOrDefault(a => a.Type == GrantSourceKey);
            if (grantSourceClaim == null) return false;
            if (grantSourceClaim.Value.IsNullOrEmpty()) return false;


            var grantTypeClaim = claims.FirstOrDefault(a => a.Type == GrantTypeKey);
            if (grantTypeClaim == null) return false;
            if (grantTypeClaim.Value.IsNullOrEmpty()) return false;


            var clientClaim = claims.FirstOrDefault(a => a.Type == ClientKey);
            if (clientClaim == null) return false;
            if (clientClaim.Value.IsNullOrEmpty()) return false;



            var notBeforeClaim = claims.FirstOrDefault(a => a.Type == NotBeforeKey);
            if (notBeforeClaim == null) return false;

            var notBeforeClaimParseResult = DateTime.TryParse(notBeforeClaim.Value, out DateTime notBefore);
            if (notBeforeClaimParseResult == false) return false;



            var issueTimeClaim = claims.FirstOrDefault(a => a.Type == IssueTimeKey);
            if (issueTimeClaim == null) return false;

            var issueTimeClaimParseResult = DateTime.TryParse(issueTimeClaim.Value, out DateTime issueTime);
            if (issueTimeClaimParseResult == false) return false;



            var expirationTimeClaim = claims.FirstOrDefault(a => a.Type == ExpirationKey);
            if (expirationTimeClaim == null) return false;

            var expirationTimeParseResult = DateTime.TryParse(expirationTimeClaim.Value, out DateTime expirationTime);
            if (expirationTimeParseResult == false) return false;


            var metadata = new Dictionary<string, string>();
            var metadataClaims = claims.Where(a => TokenPropertyKeys.Contains(a.Type) == false).ToArray();
            if (metadataClaims.NotNullAndEmpty()) metadata = metadataClaims.ToDictionary(a => a.Type, a => a.Value);


            tokenInfo = new()
            {
                UserId = userIdClaim.Value,
                Username = userIdClaim.Value,
                GrantSource = grantSourceClaim.Value,
                GrantType = grantTypeClaim.Value,
                Client = clientClaim.Value,
                Metadata = metadata,
                NotBefore = notBefore,
                IssueTime = issueTime,
                ExpirationTime = expirationTime,
            };

            return true;
        }
    }
}