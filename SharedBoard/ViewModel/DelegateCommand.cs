﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharedBoard.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _action;

        public DelegateCommand(Action action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67
    }
}