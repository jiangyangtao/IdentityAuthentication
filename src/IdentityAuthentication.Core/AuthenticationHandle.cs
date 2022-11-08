using Authentication.Abstractions;
using IdentityAuthentication.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace IdentityAuthentication.Core
{
    internal class AuthenticationHandle
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MethodInfo ExecuteAuthenticateMethod;
        private readonly MethodInfo ExecuteIdentityCheckMethod;
        private readonly IDictionary<string, Type> CredentialTypes;

        private const string GrantTypeDefault = "Password";
        private const string GrantSourceDefault = "Local";
        private const string ClientDefault = "Browser";

        public AuthenticationHandle(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var authenticationHandleType = typeof(AuthenticationHandle);
            ExecuteAuthenticateMethod = authenticationHandleType.GetMethod(nameof(ExecuteAuthenticateAsync), BindingFlags.NonPublic | BindingFlags.Instance);
            ExecuteIdentityCheckMethod = authenticationHandleType.GetMethod(nameof(ExecuteIdentityCheckAsync), BindingFlags.NonPublic | BindingFlags.Instance);

            CredentialTypes = GetCredentialTypes();
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
                        if (dic.ContainsKey(credential.GrantType)) throw new Exception($"{credential.GrantType} already exists.");

                        dic.Add(credential.GrantType, type);
                    }
                }
            }

            return dic;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(JObject credentialObject)
        {
            var credential = GetCredential(credentialObject);
            var method = ExecuteAuthenticateMethod.MakeGenericMethod(credential.GetType());
            var result = await (Task<AuthenticationResult>)method.Invoke(this, new object[] { credential });

            return result;
        }

        public async Task<bool> IdentityCheckAsync(AuthenticationResult authenticationResult)
        {
            var credentialObject = JObject.FromObject(authenticationResult);

            var credential = GetCredential(credentialObject);
            var method = ExecuteIdentityCheckMethod.MakeGenericMethod(credential.GetType());
            var result = await (Task<bool>)method.Invoke(this, new object[] { credential, authenticationResult.UserId, authenticationResult.Username });

            return result;
        }

        private ICredential GetCredential(JObject credentialObject)
        {
            if (credentialObject.ContainsKey(AuthenticationResult.GrantTypePropertyName) == false)
                credentialObject[AuthenticationResult.GrantTypePropertyName] = GrantTypeDefault;

            if (credentialObject.ContainsKey(AuthenticationResult.GrantSourcePropertyName) == false)
                credentialObject[AuthenticationResult.GrantSourcePropertyName] = GrantSourceDefault;

            if (credentialObject.ContainsKey(AuthenticationResult.ClientPropertyName) == false)
                credentialObject[AuthenticationResult.ClientPropertyName] = ClientDefault;

            var grantType = credentialObject[AuthenticationResult.GrantTypePropertyName].Value<string>();
            if (CredentialTypes.ContainsKey(grantType) == false) throw new Exception($"Unknown GrantType: {grantType}");

            var type = CredentialTypes[grantType];
            var credential = (ICredential)credentialObject.ToObject(type);

            return credential;
        }

        private async Task<AuthenticationResult?> ExecuteAuthenticateAsync<T>(T credential) where T : ICredential
        {
            var authenticationService = GetAuthenticationService(credential);

            var result = await authenticationService.AuthenticateAsync(credential);
            if (result == null) throw new Exception($"[{authenticationService.GetType().FullName}]authenticate failed, credential: {credential.GrantType}, source: {credential.GrantSource}");

            return result;
        }

        private async Task<bool> ExecuteIdentityCheckAsync<T>(T credential, string id, string username) where T : ICredential
        {
            var authenticationService = GetAuthenticationService(credential);
            return await authenticationService.IdentityCheckAsync(id, username);
        }

        private IAuthenticationService<T> GetAuthenticationService<T>(T credential) where T : ICredential
        {
            var services = _serviceProvider.GetServices<IAuthenticationService<T>>();
            if (services.Any() == false) throw new Exception($"Not support authentication type {credential.GrantSource}");

            var authenticationServices = services.Where(item => item.GrantSource.Equals(credential.GrantSource, StringComparison.OrdinalIgnoreCase)).ToArray();
            if (authenticationServices.IsNullOrEmpty()) throw new Exception($"Not map authentication source [{credential.GrantSource}]");
            if (authenticationServices.Length >= 2) throw new Exception($"Uncertain authentication source [{credential.GrantSource}]");

            var authenticationService = authenticationServices.FirstOrDefault();
            if (authenticationService == null) throw new Exception($"Authenticate failed, credential: {credential.GrantType},source: {credential.GrantSource}");

            return authenticationService;
        }
    }
}
