using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RecEpee.Framework
{
    class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<G>(G newValue, [CallerMemberName] string propertyName = null)
        {
            var oldValue = GetType().GetProperty(propertyName).GetValue(this);

            if ((oldValue == null && newValue != null) ||
                (oldValue != null && newValue == null) ||
                (newValue.Equals((G)oldValue) == false) )
            {
                var fieldName = propertyToFieldName(propertyName);

                GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(this, newValue);
                RaisePropertyChanged(propertyName);
            }
        }

        private static string propertyToFieldName(string propertyName)
        {
            return "_" + Char.ToLower(propertyName[0], CultureInfo.InvariantCulture) + propertyName.Substring(1);
        }
    }
}
