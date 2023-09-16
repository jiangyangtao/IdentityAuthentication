
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
