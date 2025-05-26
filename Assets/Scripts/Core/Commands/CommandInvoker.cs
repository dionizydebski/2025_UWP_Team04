using System.Collections.Generic;
using UnityEngine;

namespace Core.Commands
{
    public class CommandInvoker
    {
        private readonly Stack<ICommand> commandHistory = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            commandHistory.Push(command);
        }

        public void Undo()
        {
            if (commandHistory.Count > 0)
            {
                ICommand lastCommand = commandHistory.Pop();
                Debug.Log("Undoing command: " + lastCommand.GetType().Name);
                lastCommand.Undo();
            }
            else
            {
                Debug.Log("No commands to undo");
            }
        }
    }

}