using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BGH_Kompakt.Commands
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> _executeHandler;
        private readonly Predicate<object> _canExecuteHandler;
        private Action<object, ExecutedRoutedEventArgs> saveExecuted;

        public RelayCommand(Action<object> execute) : this(execute, null)
        { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("Execute cannot be null");
            _executeHandler = execute;
            _canExecuteHandler = canExecute;
        }

        public RelayCommand(Action<object, ExecutedRoutedEventArgs> saveExecuted)
        {
            this.saveExecuted = saveExecuted;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _executeHandler(parameter);
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteHandler == null) return true;
            return _canExecuteHandler(parameter);
        }
    }
}
