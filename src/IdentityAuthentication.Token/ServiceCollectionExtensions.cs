using IdentityAuthentication.Token.Abstractions;
using IdentityAuthentication.Token.TokenEncryption;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityAuthentication.Token
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddToken(this IServiceCollection services)
        {
            services.AddSingleton<ITokenProvider, JwtTokenProvider>();
            services.AddSingleton<ITokenProvider, ReferenceTokenProvider>();
            services.AddSingleton<ITokenProvider, EncryptionTokenProvider>();
            services.AddSingleton<ITokenProviderFactory, TokenProviderFactory>();

            services.AddSingleton<ITokenEncryptionProviderFactory, TokenEncryptionProviderFactory>();
            services.AddSingleton<ITokenAesEncryptionProvider, TokenAesEncryptionProvider>();
            services.AddSingleton<ITokenRsaEncryptionProvider, TokenRsaEncryptionProvider>();

            services.AddSingleton<ITokenValidationProvider, TokenValidationProvider>();

            return services;
        }
    }
}
