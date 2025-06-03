using System.Collections.Generic;
using Core.Commands;
using Singleton;
using UnityEngine;

namespace Core
{
    public class CommandInvoker : Singleton<CommandInvoker>
    {
        private readonly Stack<ICommand> _commandHistory = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _commandHistory.Push(command);
        }

        public void Undo()
        {
            if (_commandHistory.Count > 0)
            {
                ICommand lastCommand = _commandHistory.Pop();
                lastCommand.Undo();
            }
        }
    }
}