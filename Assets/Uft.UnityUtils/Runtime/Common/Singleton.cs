#nullable enable

namespace Uft.UnityUtils.Common
{
    public class Singleton<T> where T : class, new()
    {
        public static T Instance => instance ??= new T();
        static T? instance;
        public static void Recreate() => instance = new T();
    }
}
