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

        private const string AuthenticationTypeKey = nameof(AuthenticationResult.AuthenticationType);
        private const string AuthenticationSourceKey = nameof(AuthenticationResult.AuthenticationSource);
        private const string AuthenticationTypeDefault = "Password";
        private const string AuthenticationSourceDefault = "Local";

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
                        if (dic.ContainsKey(credential.AuthenticationType)) throw new Exception($"{credential.AuthenticationType} already exists.");

                        dic.Add(credential.AuthenticationType, type);
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
            var credentialObject = new JObject {
                new JProperty(AuthenticationTypeKey,authenticationResult.AuthenticationType),
                new JProperty(AuthenticationSourceKey,authenticationResult.AuthenticationSource),
            };

            var credential = GetCredential(credentialObject);
            var method = ExecuteIdentityCheckMethod.MakeGenericMethod(credential.GetType());
            var result = await (Task<bool>)method.Invoke(this, new object[] { credential, authenticationResult.UserId, authenticationResult.Username });

            return result;
        }

        private ICredential GetCredential(JObject credentialObject)
        {
            if (credentialObject.ContainsKey(AuthenticationTypeKey) == false)
                credentialObject[AuthenticationTypeKey] = AuthenticationTypeDefault;

            if (credentialObject.ContainsKey(AuthenticationSourceKey) == false)
                credentialObject[AuthenticationSourceKey] = AuthenticationSourceDefault;

            var authenticationType = credentialObject[AuthenticationTypeKey].Value<string>();
            if (CredentialTypes.ContainsKey(authenticationType) == false) throw new Exception($"Unknown AuthenticationType: {authenticationType}");

            var type = CredentialTypes[authenticationType];
            var credential = (ICredential)credentialObject.ToObject(type);

            return credential;
        }

        private async Task<AuthenticationResult?> ExecuteAuthenticateAsync<T>(T credential) where T : ICredential
        {
            var authenticationService = GetAuthenticationService(credential);

            var result = await authenticationService.AuthenticateAsync(credential);
            if (result == null) throw new Exception($"[{authenticationService.GetType().FullName}]authenticate failed, credential: {credential.AuthenticationType}, source: {credential.AuthenticationSource}");

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
            if (services.Any() == false) throw new Exception($"Not support authentication type {credential.AuthenticationSource}");

            var authenticationServices = services.Where(item => item.AuthenticationSource.Equals(credential.AuthenticationSource, StringComparison.OrdinalIgnoreCase)).ToArray();
            if (authenticationServices.IsNullOrEmpty()) throw new Exception($"Not map authentication source [{credential.AuthenticationSource}]");
            if (authenticationServices.Length >= 2) throw new Exception($"Uncertain authentication source [{credential.AuthenticationSource}]");

            var authenticationService = authenticationServices.FirstOrDefault();
            if (authenticationService == null) throw new Exception($"Authenticate failed, credential: {credential.AuthenticationType},source: {credential.AuthenticationSource}");

            return authenticationService;
        }
    }
}
