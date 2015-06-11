using System;
using System.Globalization;
using System.Windows.Input;

namespace RecEpee.Framework
{
    class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public string Name { get; private set; }
        public KeyGesture Shortcut { get; private set; }
        public string ShortcutText
        {
            get { return Shortcut.GetDisplayStringForCulture(CultureInfo.CurrentUICulture); }
        }


        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _canExecute = canExecute;
            _execute = execute;
        }

        public RelayCommand(string name, Action<object> execute, Predicate<object> canExecute = null)
            : this(execute, canExecute)
        {
            Name = name;
        }

        public RelayCommand(KeyGesture shortcut, Action<object> execute, Predicate<object> canExecute = null)
            : this(execute, canExecute)
        {
            Shortcut = shortcut;
        }

        public RelayCommand(string name, KeyGesture shortcut, Action<object> execute, Predicate<object> canExecute = null)
            : this(name, execute, canExecute)
        {
            Shortcut = shortcut;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
