using UI;
using UnityEngine;

namespace Core.Commands
{
    public class UndoButtonController : MonoBehaviour
    {
        [SerializeField] private PlayerActionsController _playerActionsController;

        public void OnUndoButtonClick()
        {
            Debug.Log("Undo clicked");
            _playerActionsController.OnUndoPressed();
        }
    }
}