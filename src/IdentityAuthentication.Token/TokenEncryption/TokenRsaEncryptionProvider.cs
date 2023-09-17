using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Token.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Token.TokenEncryption
{
    internal class TokenRsaEncryptionProvider : ITokenEncryptionProvider
    {
        public TokenRsaEncryptionProvider()
        {
        }

        public TokenEncryptionType EncryptionType => TokenEncryptionType.Rsa;
    }
}
