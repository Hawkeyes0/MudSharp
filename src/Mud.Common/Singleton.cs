using System;

namespace Mud.Common
{
    public static class Singleton<T>
    {
        private static T _instance;

        static Singleton()
        {
            Type type = typeof(T);
            if (type.IsClass)
            {
                var constructor = type.GetConstructor(new Type[]{});
                _instance = (T)constructor.Invoke(null);
            }
            else
            {
                _instance = default(T);
            }
        }

        public static T Instance => _instance;
    }
}
