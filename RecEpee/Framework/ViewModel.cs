using System.Runtime.CompilerServices;

namespace RecEpee.Framework
{
    class ViewModel<T> : ViewModelBase where T : new()
    {
        private T _model;
        public T Model
        {
            get { return _model; }
            set { _model = value; OnBindModel(); }
        }

        public ViewModel()
        {
            Model = new T();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ViewModel(T model)
        {
            Model = model;
        }

        protected void SetModelProperty<V>(V newValue, [CallerMemberName] string propertyName = null)
        {
            var oldValue = Model.GetType().GetProperty(propertyName).GetValue(Model);

            if ((oldValue == null && newValue != null) ||
                (oldValue != null && newValue == null) ||
                (newValue.Equals((V)oldValue) == false))
            {
                Model.GetType().GetProperty(propertyName).SetValue(Model, newValue);
                RaisePropertyChanged(propertyName);
            }
        }

        protected virtual void OnBindModel() { }
    }
}
