using IdentityAuthenticaion.Model.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.TokenServices.Abstractions
{
    public interface ITokenProviderFactory
    {
        public ITokenProvider CreateTokenService();
    }
}
