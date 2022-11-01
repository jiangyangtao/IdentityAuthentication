using IdentityAuthenticaion.Model.Configurations;
using IdentityAuthentication.TokenServices.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.TokenServices.Services
{
    internal class ReferenceTokenService : ITokenService
    {
        public ReferenceTokenService()
        {
        }

        public TokenType TokenType => TokenType.Reference;
    }
}
