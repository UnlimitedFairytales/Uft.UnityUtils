namespace Assets.Uft.UnityUtils.Runtime.Common
{
    public class Singleton<T> where T : new()
    {
        static readonly object _lock = new();
        static T _instance;

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    _instance ??= new();
                }
                return _instance;
            }
        }
    }
}
