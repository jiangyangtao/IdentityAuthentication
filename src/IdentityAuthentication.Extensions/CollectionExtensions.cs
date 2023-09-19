
namespace IdentityAuthentication.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> values) => values == null || values.Any() == false;

        public static bool IsNullOrEmpty<T>(this IReadOnlyCollection<T> values) => values == null || values.Count == 0;

        public static bool NotNullAndEmpty<T>(this IReadOnlyCollection<T> values) => IsNullOrEmpty(values) == false;
    }
}
