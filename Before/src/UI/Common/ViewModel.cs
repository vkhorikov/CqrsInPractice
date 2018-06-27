using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace UI.Common
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        protected static readonly DialogService _dialogService = new DialogService();
        private bool? _dialogResult;

        public bool? DialogResult
        {
            get => _dialogResult;
            protected set
            {
                _dialogResult = value;
                Notify();
            }
        }

        public virtual string Caption => string.Empty;
        public virtual double Height => 300;
        public virtual double Width => 600;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void Notify<T>(Expression<Func<T>> propertyExpression)
        {
            if (!(propertyExpression.Body is MemberExpression expression))
                throw new ArgumentException(propertyExpression.Body.ToString());

            Notify(expression.Member.Name);
        }
    }
}
