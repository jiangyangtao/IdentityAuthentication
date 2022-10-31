using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthenticaion.Model.Configurations
{
    public class AuthenticationConfiguration
    {
        public string EncryptionAlgorithm { set; get; } = SecurityAlgorithms.RsaSha256;

        /// <summary>
        /// Enable grpc connection
        /// </summary>
        public bool EnableGrpcConnection { get; set; } = true;

        public bool EnableTokenEncrypt { get; set; } = false;
    }
}
