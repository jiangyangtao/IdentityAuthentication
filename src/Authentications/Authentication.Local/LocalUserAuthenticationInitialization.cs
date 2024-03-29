﻿using Authentication.Abstractions;
using Authentication.Abstractions.Credentials;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Local
{
    internal class LocalUserAuthenticationInitialization : IAuthenticationInitialization
    {
        public IApplicationBuilder InitializationApplication(IApplicationBuilder builder) => builder;

        public IServiceCollection InitializationService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAuthenticationService<PasswordCredential>, LocalUserAuthenticationService>();
            return services;
        }
    }
}
