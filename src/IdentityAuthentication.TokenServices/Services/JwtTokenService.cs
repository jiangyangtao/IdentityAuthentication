using IdentityAuthenticaion.Model.Configurations;
using IdentityAuthentication.TokenServices.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.TokenServices.Services
{
    internal class JwtTokenService : ITokenService
    {
        public JwtTokenService()
        {
        }

        public TokenType TokenType => TokenType.JWT;
    }
}
