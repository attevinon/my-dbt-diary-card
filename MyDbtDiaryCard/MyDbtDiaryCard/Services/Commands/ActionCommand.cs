using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MyDbtDiaryCard.Services.Commands
{
    internal class ActionCommand : ICommand
    {
        private Action action;
        public event EventHandler CanExecuteChanged;
        public ActionCommand(Action act)
        {
            action = act;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action.Invoke();
        }
    }
}
