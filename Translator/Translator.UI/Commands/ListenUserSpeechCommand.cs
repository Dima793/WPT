using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Translator.UI.Commands
{
    class ListenUserSpeechCommand : ICommand
    {
        private readonly Action<object> _execute;

        private readonly Predicate<object> _canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return this._canExecute != null && this._canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this._execute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public ListenUserSpeechCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }
    }
}
