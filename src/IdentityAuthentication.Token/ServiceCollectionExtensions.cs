﻿using IdentityAuthentication.Token.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return services;
        }
    }
}