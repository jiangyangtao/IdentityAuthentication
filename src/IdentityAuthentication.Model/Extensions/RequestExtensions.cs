using IdentityAuthentication.Model.Handles;
using System.Net.Http.Headers;

namespace IdentityAuthentication.Model.Extensions
{
    public static class RequestExtensions
    {
        public static void SetRefreshToken(this HttpRequestHeaders headers, string refreshToken) => headers.SetValue(IdentityAuthenticationDefaultKeys.RefreToken, refreshToken);

        public static void SetAccessToken(this HttpRequestHeaders headers, string accessToken) => headers.SetValue(IdentityAuthenticationDefaultKeys.AccessToken, accessToken);

        public static void SetAuthorization(this HttpRequestHeaders headers, string authorization) => headers.SetValue(IdentityAuthenticationDefaultKeys.Authorization, authorization);

        public static void SetValue(this HttpRequestHeaders headers, string key, string value)
        {
            if (headers == null) return;
            if (key.IsNullOrEmpty()) return;
            if (value.IsNullOrEmpty()) return;

            if (headers.Contains(key))
            {
                headers.Authorization = new AuthenticationHeaderValue(value);
                return;
            }

            headers.Add(key, value);
        }
    }
}
