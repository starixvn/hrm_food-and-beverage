// --------------------------------------------------------------------------
// <copyright file="RelayCommand.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.16">Created</change>
// </history>

namespace NexFB.WinAppSmartCard.Core.Commands
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Relay commands.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public RelayCommand(Predicate<object> canExecute, Action<object> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _canExecute = canExecute;
            _execute = execute;
        }

        public RelayCommand(Action<object> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((object)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((object)parameter);
        }
    }
}
