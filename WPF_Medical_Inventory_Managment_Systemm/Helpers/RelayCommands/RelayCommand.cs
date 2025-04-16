using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_Medical_Inventory_Managment_Systemm.Helpers.RelayCommands
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Func<T, Task> _execute;
        private readonly Predicate<T> _canExecute;

        public RelayCommand(Func<T, Task> execute, Predicate<T> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public async void Execute(object parameter)
        {
            await _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
