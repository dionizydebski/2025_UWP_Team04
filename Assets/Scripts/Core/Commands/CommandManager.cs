using System.Collections.Generic;

namespace Core.Commands
{
    public class CommandManager
    {
        private readonly Stack<ICommand> undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> redoStack = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            undoStack.Push(command);
            redoStack.Clear(); // Reset redo po nowym działaniu
        }

        public void Undo()
        {
            if (undoStack.Count == 0) return;

            ICommand command = undoStack.Pop();
            command.Undo();
            redoStack.Push(command);
        }

        public void Redo()
        {
            if (redoStack.Count == 0) return;

            ICommand command = redoStack.Pop();
            command.Execute();
            undoStack.Push(command);
        }

        public void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
        }
    }
}