using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> values) => values == null || values.Count == 0;

        public static bool IsNullOrEmpty<T>(this T[] values) => values == null || values.Length == 0;

        public static bool IsNullOrEmpty<T>(this IReadOnlyCollection<T> values) => values == null || values.Count == 0;

        public static bool NotNullAndEmpty<T>(this IReadOnlyCollection<T> values) => IsNullOrEmpty(values) == false;
    }
}
