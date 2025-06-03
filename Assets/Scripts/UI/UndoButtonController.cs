using UnityEngine;

namespace UI
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