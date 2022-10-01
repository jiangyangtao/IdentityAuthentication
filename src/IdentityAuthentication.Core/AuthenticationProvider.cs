using Authentication.Abstractions;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace IdentityAuthentication.Core
{
    internal class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AuthenticationProvider> _logger;

        private readonly IDictionary<string, Type> CredentialTypes;
        private readonly MethodInfo GenericAuthenticationMethod;

        private const string AuthenticationTypeKey = "AuthenticationType";
        private const string AuthenticationSourceKey = "AuthenticationSource";
        private const string AuthenticationTypeDefault = "Password";
        private const string AuthenticationSourceDefault = "Local";

        public AuthenticationProvider(
            ITokenProvider tokenProvider,
            IServiceProvider serviceProvider,
            ILogger<AuthenticationProvider> logger)
        {
            _tokenProvider = tokenProvider;
            _serviceProvider = serviceProvider;
            _logger = logger;

            CredentialTypes = GetCredentialTypes();
            GenericAuthenticationMethod = typeof(AuthenticationProvider).GetMethod(nameof(ExecuteAuthenticateAsync), BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private static IDictionary<string, Type> GetCredentialTypes()
        {
            var assembly = AssemblyProvider.Authentications.FirstOrDefault(a => a.FullName.Contains("Abstractions"));
            var credentialType = typeof(ICredential);
            var types = assembly.GetTypes();

            var dic = new Dictionary<string, Type>();
            foreach (var type in types)
            {
                if (type.HasInterface(credentialType))
                {
                    var credential = (ICredential)Activator.CreateInstance(type);
                    if (credential != null)
                    {
                        if (dic.ContainsKey(credential.AuthenticationType)) throw new Exception($"{credential.AuthenticationType} already exists.");

                        dic.Add(credential.AuthenticationType, type);
                    }
                }
            }

            return dic;
        }

        public async Task<IToken> AuthenticateAsync(JObject credentialObject)
        {
            var credential = GetCredential(credentialObject);
            var result = await GetAuthenticationResultAsync(credential);
            var token = _tokenProvider.GenerateToken(result.UserId, result.ToJObject());

            return await Task.FromResult(token);
        }


        public IToken RefreshToken()
        {
            return _tokenProvider.RefreshToken();
        }

        public async Task<IToken> RefreshTokenAsync()
        {
            return await _tokenProvider.RefreshTokenAsync();
        }

        private ICredential GetCredential(JObject credentialObject)
        {
            var authenticationType = credentialObject[AuthenticationTypeKey].Value<string>();
            if (authenticationType.IsNullOrEmpty()) credentialObject[AuthenticationTypeKey] = AuthenticationTypeDefault;

            var authenticationSource = credentialObject[AuthenticationSourceKey].Value<string>();
            if (authenticationSource.IsNullOrEmpty()) credentialObject[AuthenticationSourceKey] = AuthenticationSourceDefault;

            if (CredentialTypes.ContainsKey(authenticationType) == false) throw new Exception($"Unknown AuthenticationType: {authenticationType}");

            var type = CredentialTypes[authenticationType];
            var credential = (ICredential)credentialObject.ToObject(type);

            return credential;
        }

        private async Task<AuthenticationResult> GetAuthenticationResultAsync(ICredential credential)
        {
            var method = GenericAuthenticationMethod.MakeGenericMethod(credential.GetType());
            var result = await (Task<AuthenticationResult>)method.Invoke(this, new object[] { credential });

            return result;
        }

        private async Task<AuthenticationResult?> ExecuteAuthenticateAsync<T>(T credential) where T : ICredential
        {
            var services = _serviceProvider.GetServices<IAuthenticationService<T>>();
            if (services.Any() == false) throw new Exception($"Not support authentication type {credential.AuthenticationSource}");

            var authenticationServices = services.Where(item => item.AuthenticationSource.Equals(credential.AuthenticationSource, StringComparison.OrdinalIgnoreCase)).ToArray();
            if (authenticationServices.IsNullOrEmpty()) throw new Exception($"Not map authentication source [{credential.AuthenticationSource}]");
            if (authenticationServices.Length >= 2) throw new Exception($"Uncertain authentication source [{credential.AuthenticationSource}]");

            var authenticationService = authenticationServices.FirstOrDefault();
            if (authenticationService == null) throw new Exception($"authenticate failed, credential: {credential},source: {credential.AuthenticationSource}");

            var result = await authenticationService.AuthenticateAsync(credential);
            if (result == null) throw new Exception($"[{authenticationService.GetType().FullName}]authenticate failed, credential: {credential}, source: {credential.AuthenticationSource}");

            return result;
        }

    }
}
