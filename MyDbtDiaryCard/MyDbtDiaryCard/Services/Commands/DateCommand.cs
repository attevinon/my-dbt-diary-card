using System;
using System.Windows.Input;

namespace MyDbtDiaryCard.Services.Commands
{
    internal class DateCommand : ICommand
    {
        private Action action;
        public event EventHandler CanExecuteChanged;
        public DateCommand(Action act)
        {
            action = act;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
                return false;

            DateTime date = (DateTime)parameter;

            if(date < DateTime.Today && date > DateTime.MinValue)
                return true;

            return false;
        }

        public void Execute(object parameter)
        {
            action.Invoke();
        }
    }
}
