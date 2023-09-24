using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using IdentityAuthentication.Model.Extensions;
using IdentityAuthentication.Model.Handles;
using Newtonsoft.Json;
using System.Security.Claims;

namespace IdentityAuthentication.Token
{
    internal class TokenInfo : IAuthenticationResult
    {
        public TokenInfo()
        {
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

        public static string NotBeforeKey = IdentityAuthenticationDefaultKeys.NotBefore;

        public static string IssueTimeKey = IdentityAuthenticationDefaultKeys.IssueTime;

        public static string ExpirationKey = IdentityAuthenticationDefaultKeys.Expiration;

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

        #region Methods

        /// <summary>
        /// Build to json
        /// </summary>
        /// <returns></returns>
        public override string ToString() => JsonConvert.SerializeObject(this);

        public string ToJson() => JsonConvert.SerializeObject(this);

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
            if (token.IsEmpty == false) return false;

            accessToken = token;
            return true;
        }

        public bool IsEmpty
        {
            get
            {
                if (UserId.IsNullOrEmpty()) return false;
                if (Username.IsNullOrEmpty()) return false;
                if (GrantSource.IsNullOrEmpty()) return false;
                if (GrantType.IsNullOrEmpty()) return false;
                if (Client.IsNullOrEmpty()) return false;

                return true;
            }
        }

        public IAuthenticationResult GetAuthenticationResult() => this;


        public IReadOnlyDictionary<string, string> BuildDictionary()
        {
            var dic = new Dictionary<string, string>
            {
                {UserIdKey, UserId},
                {UsernameKey,Username},
                {GrantTypeKey,GrantType},
                {GrantSourceKey,GrantSource},
                {ClientKey,Client},
            };
            if (Metadata.IsNullOrEmpty()) return dic;

            foreach (var item in Metadata)
            {
                dic.Add(item.Key, item.Value);
            }

            return dic;
        }

        public bool Equals(TokenInfo tokenInfo)
        {
            if (UserId != tokenInfo.UserId) return false;
            if (Username != tokenInfo.Username) return false;
            if (GrantSource != tokenInfo.GrantSource) return false;
            if (GrantType != tokenInfo.GrantType) return false;
            if (Client != tokenInfo.Client) return false;

            return true;
        }

        #endregion

        #region Static Methods  

        public static TokenInfo CreateToken(IAuthenticationResult result) => new()
        {
            UserId = result.UserId,
            Username = result.Username,
            GrantSource = result.GrantSource,
            GrantType = result.GrantType,
            Client = result.Client,
            Metadata = result.Metadata
        };

        public static bool TryParse(IDictionary<string, object> claimDictionary, out TokenInfo? tokenInfo)
        {
            tokenInfo = null;
            if (claimDictionary.IsNullOrEmpty()) return false;

            var claims = claimDictionary.Select(a => new Claim(a.Key, a.Value.ToString())).ToArray();
            return TryParse(claims, out tokenInfo);
        }

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

        #endregion
    }
}
