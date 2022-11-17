﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace IdentityAuthentication.Model.Extensions
{
    public static class HttpHeaderExtensions
    {
        public static string GetRefreshToken(this IHeaderDictionary headers) => headers.GetValue(HttpHeaderKeyDefaults.RefreToken);

        public static string GetAuthorization(this IHeaderDictionary headers) => headers.GetValue(HttpHeaderKeyDefaults.Authorization);

        public static string GetValue(this IHeaderDictionary headers, string key, string removeChar = "Bearer ")
        {
            if (headers == null) return string.Empty;
            if (key.IsNullOrEmpty()) return string.Empty;

            var r = headers.TryGetValue(HttpHeaderKeyDefaults.Authorization, out StringValues _value);
            if (r == false) return string.Empty;

            var value = _value.ToString();
            if (value.StartsWith(removeChar, StringComparison.OrdinalIgnoreCase))
            {
                value = value[removeChar.Length..].Trim();
            }

            return value;
        }

        public static void SetRefreshToken(this IHeaderDictionary headers, string refreshToken) => headers.SetValue(HttpHeaderKeyDefaults.RefreToken, refreshToken);

        public static void SetAccessToken(this IHeaderDictionary headers, string accessToken) => headers.SetValue(HttpHeaderKeyDefaults.AccessToken, accessToken);

        public static void SetAuthorization(this IHeaderDictionary headers, string authorization) => headers.SetValue(HttpHeaderKeyDefaults.Authorization, authorization);


        public static void SetValue(this IHeaderDictionary headers, string key, string value)
        {
            if (headers == null) return;
            if (key.IsNullOrEmpty()) return;
            if (value.IsNullOrEmpty()) return;

            if (headers.ContainsKey(key))
            {
                headers[key] = value;
                return;
            }

            headers.Add(key, value);
        }
    }
}
