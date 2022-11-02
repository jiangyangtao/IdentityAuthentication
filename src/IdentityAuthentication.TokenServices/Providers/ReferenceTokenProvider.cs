using Authentication.Abstractions;
using IdentityAuthenticaion.Model.Configurations;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.TokenServices.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.TokenServices.Providers
{
    internal class ReferenceTokenProvider : ITokenProvider
    {
        private readonly ICacheProvider _cacheProvider;

        public ReferenceTokenProvider(ICacheProviderFactory cacheProviderFactory, IOptions<CacheStorageConfiguration> options)
        {
            _cacheProvider = cacheProviderFactory.CreateCacheProvider(options.Value.StorageType);
        }

        public TokenType TokenType => TokenType.Reference;

        public Task<string> DestroyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IToken> GenerateAsync(AuthenticationResult authenticationResult)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticationResult> GetAuthenticationResultAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> RefreshAsync()
        {
            throw new NotImplementedException();
        }
    }
}
