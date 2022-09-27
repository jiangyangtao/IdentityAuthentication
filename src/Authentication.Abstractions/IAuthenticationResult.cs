using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Abstractions
{
    public interface IAuthenticationResult
    {
        public string UserId { get; }

        public string AuthenticationSource { get; }

        /// <summary>
        /// 元数据
        /// </summary>
        public IReadOnlyDictionary<string, string> Metadata { get; }
    }
}
