
namespace IdentityAuthentication.Extensions
{
    public static class TypeExtensions
    {
        public static bool HasInterface(this Type type, string name)
        {
            if (name.IsNullOrEmpty()) return false;

            var interfaceType = type.GetInterface(name);
            return interfaceType != null;
        }

        public static bool HasInterface(this Type type, Type interfaceType)
        {
            if (type == null) return false;
            if (interfaceType == null) return false;

            return type.GetInterfaces().Any(a => a == interfaceType);
        }

        public static bool HasInterface<T>(this Type type)
        {
            if (type == null) return false;

            return HasInterface(type, typeof(T));
        }
    }
}
