using System;
using Core;
using Core.Commands;
using Tower;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class PlayerActionsController : MonoBehaviour
    {
        private TowerManager _towerManager;

        private Vector3 _mousePosition;
        private float _zAxis = 0.2f;
        private bool _isTowerSelected;
        private float _towerRadius;
        private int _towerRange;

        [Header("LayerMasks for raycasting")]
        [SerializeField] private LayerMask boardMask;
        [SerializeField] private LayerMask towerMask;
        [SerializeField] private LayerMask uiMask;

        [Header("Tower Prefabs")]
        [SerializeField] private RangeTower shootingTowerPrefab;
        [SerializeField] private SlowingTower slowingTowerPrefab;

        [Header("Keys for placing towers")] 
        [SerializeField] private KeyCode selectShootingTowerCode;
        [SerializeField] private KeyCode selectSlowingTowerCode;

        private BaseTower _selectedTower;
        private BaseTower _towerToPlace;

        [Header("References")]
        [SerializeField] private PlayerActionsView playerActionsView;
        [SerializeField] private TowerManagementPanel towerManagementPanel;
        [SerializeField] private UndoButtonController undoButtonController;  // <-- Undo controller reference

        private void Awake()
        {
            Cursor.visible = true;
            _towerManager = TowerManager.Instance;
            if (_towerManager == null)
            {
                Debug.LogError("TowerManager.Instance is null!");
            }
            playerActionsView.CreateRadiusAndRangeWidget(GetMouseWorldPosition(), 1, 1);
        }

        private void Update()
        {
            if (_towerToPlace)
            {
                PlaceTower();
            }
            else if (Input.GetKeyDown(selectShootingTowerCode))
            {
                SelectTowerToPlace(shootingTowerPrefab);
            }
            else if (Input.GetKeyDown(selectSlowingTowerCode))
            {
                SelectTowerToPlace(slowingTowerPrefab);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                SelectTower();
            }

            if (!_isTowerSelected)
            {
                playerActionsView.HideRadiusAndRangeWidget();
            }
        }

        private void PlaceTower()
        {
            if (_towerToPlace == null)
            {
                Debug.LogError("_towerToPlace is null!");
                return;
            }
            if (_towerManager == null)
            {
                Debug.LogError("_towerManager is null!");
                return;
            }

            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit,
                    Mathf.Infinity, boardMask) || !_isTowerSelected) return;

            playerActionsView.ShowRadiusAndRangeWidget();
            playerActionsView.MoveCircleToMouse(hit.point);

            if (!_towerManager.CanPlaceTower(_towerToPlace, hit.point))
            {
                playerActionsView.SetRadiusWidgetUnableToPlaceColor();
            }
            else
            {
                playerActionsView.SetRadiusWidgetDefaultColor();

                if (Input.GetMouseButtonDown(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject()) return;

                    PlaceTowerCommand command = new PlaceTowerCommand(_towerToPlace, hit.point);
                    undoButtonController.ExecuteCommand(command);

                    _isTowerSelected = false;
                    _towerToPlace = null;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                playerActionsView.HideRadiusAndRangeWidget();
                _isTowerSelected = false;
                _towerToPlace = null;
                TutorialEventsManager.Instance.TriggerTutorialStepEvent(TutorialEventsManager.PlaceTowerTutorialName, 0);
            }
        }

        private void SelectTower()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, towerMask))
            {
                BaseTower baseTower = hit.collider.GetComponentInParent<BaseTower>();

                if (baseTower != null && baseTower != _selectedTower)
                {
                    ClearSelect();

                    _selectedTower = baseTower;

                    playerActionsView.ShowTowerRangeIndicator(_selectedTower);
                    _towerManager.SelectTower(_selectedTower);

                    towerManagementPanel.OpenPanel(_selectedTower);
                }
            }
            else
            {
                ClearSelect();
                _towerManager.UnselectTower();
            }
        }

        private void ClearSelect()
        {
            if (_selectedTower)
            {
                playerActionsView.HideTowerRangeIndicator();
                towerManagementPanel.ClosePanel();
            }
            _selectedTower = null;
        }

        public void SelectTowerToPlace(BaseTower towerToPlace)
        {
            _towerToPlace = towerToPlace;
            CapsuleCollider capsuleCollider = _towerToPlace.GetComponentInChildren<CapsuleCollider>();

            if (capsuleCollider == null) return;
            _isTowerSelected = true;
            _towerRadius = capsuleCollider.radius;
            _towerRange = towerToPlace.GetBaseRange();

            playerActionsView.SetRadiusWidgetDefaultColor();
            playerActionsView.SetRadiusAndRangeWidgetSize(_towerRadius, _towerRange);
            playerActionsView.ShowRadiusAndRangeWidget();
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = _zAxis;
            return Camera.main.ScreenToWorldPoint(mousePosition);
        }

        // Metoda wywoływana z UI — np. przycisk "Sell Tower"
        public void OnSellTowerButtonClicked()
        {
            if (_selectedTower != null)
            {
                SellTowerCommand command = new SellTowerCommand(_selectedTower);
                undoButtonController.ExecuteCommand(command);

                ClearSelect();
                _towerManager.UnselectTower();
            }
        }
    }
}
