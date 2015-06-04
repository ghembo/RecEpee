using System;
using System.Windows;

namespace RecEpee.Framework
{
    /// <summary>
    /// Binds a ViewModel to a View
    /// </summary>
    static class VvmBinder
    {
        public static void RegisterBinding<Vm, V>()
        {
            Implementation<Vm>.ViewType = typeof(V);
        }

        public static Window GetView<Vm>()
        {
            var view = (Window)Activator.CreateInstance(Implementation<Vm>.ViewType);
            view.DataContext = Activator.CreateInstance(typeof(Vm));

            return view;
        }

        private static class Implementation<T>
        {
            internal static Type ViewType;
        }
    }
}
