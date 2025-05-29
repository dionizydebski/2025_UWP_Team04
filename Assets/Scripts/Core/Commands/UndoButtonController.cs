using Core;
using Core.Commands;
using UnityEngine;

namespace UI
{
    public class UndoButtonController : MonoBehaviour
    {
        private CommandInvoker invoker;

        private void Awake()
        {
            invoker = new CommandInvoker();
        }

        public void ExecuteCommand(ICommand command)
        {
            invoker.ExecuteCommand(command);
        }

        public void OnUndoButtonClick()
        {
            Debug.Log("Undo clicked");
            invoker.Undo();
        }
    }
}