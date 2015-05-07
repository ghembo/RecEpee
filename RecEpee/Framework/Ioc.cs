
namespace RecEpee.Framework
{
    static class Ioc
    {
        public static void RegisterInstance<T>(T implementation)
        {
            Implementation<T>.Instance = implementation;
        }

        public static T GetInstance<T>()
        {
            return Implementation<T>.Instance;
        }
        
        private static class Implementation<T>
        {
            internal static T Instance;
        }
    }    
}
