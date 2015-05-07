using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace RecEpee.Framework
{
    class WindowClosingBehavior
    {
        public static ICommand GetClosing(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ClosingProperty);
        }

        public static void SetClosing(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ClosingProperty, value);
        }

        public static readonly DependencyProperty ClosingProperty = DependencyProperty.RegisterAttached("Closing",
            typeof(ICommand), typeof(WindowClosingBehavior), new UIPropertyMetadata(new PropertyChangedCallback(ClosingChanged)));

        private static void ClosingChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            Window window = target as Window;

            if (window != null)
            {
                if (e.NewValue != null)
                {
                    window.Closing += Window_Closing;
                }
                else
                {
                    window.Closing -= Window_Closing;
                }
            }
        }
        
        static void Window_Closing(object sender, CancelEventArgs e)
        {
            ICommand closing = GetClosing(sender as Window);

            if (closing != null)
            {
                if (closing.CanExecute(null))
                {
                    closing.Execute(null);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
