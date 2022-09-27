using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Extensions
{
    public static class StringExtensions
    {
        #region 指示指定的字符串是 null 还是 System.String.Empty 字符串
        /// <summary>
        /// 指示指定的字符串是 null 还是 <see cref="string.Empty"/> 字符串
        /// </summary>
        /// <param name="value">要检测的字符串</param>
        /// <returns>如果字符串为 null 或空字符串 ("")，则为 true；否则为 false。</returns>
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
        #endregion

        #region 指示指定的字符串不为 null 和 System.String.Empty 字符串
        /// <summary>
        /// 指示指定的字符串不为 null 和不为 <see cref="string.Empty"/> 字符串
        /// </summary>
        /// <param name="value">要检测的字符串</param>
        /// <returns>如果字符串不为 null 和空字符串 ("")，则为 true；否则为 false。</returns>
        public static bool NotNullAndEmpty(this string value) => !string.IsNullOrEmpty(value);
        #endregion
    }
}
