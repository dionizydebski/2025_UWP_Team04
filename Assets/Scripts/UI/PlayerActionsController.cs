using System.Collections;
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

        [Header("LayerMasks")]
        [SerializeField] private LayerMask boardMask;
        [SerializeField] private LayerMask towerMask;
        [SerializeField] private LayerMask uiMask;

        [Header("Tower Prefabs")]
        [SerializeField] private RangeTower shootingTowerPrefab;
        [SerializeField] private SlowingTower slowingTowerPrefab;

        [Header("Tower Keys")]
        [SerializeField] private KeyCode selectShootingTowerCode;
        [SerializeField] private KeyCode selectSlowingTowerCode;

        private BaseTower _selectedTower;
        private BaseTower _towerToPlace;

        [Header("References")]
        [SerializeField] private PlayerActionsView playerActionsView;
        [SerializeField] private TowerManagementPanel towerManagementPanel;

        private void Awake()
        {
            Cursor.visible = true;
            _towerManager = TowerManager.Instance;

            if (_towerManager == null)
                Debug.LogError("TowerManager.Instance is null!");

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
                if (!EventSystem.current.IsPointerOverGameObject())
                    SelectTower();
            }

            if (!_isTowerSelected)
                playerActionsView.HideRadiusAndRangeWidget();
        }

        private void PlaceTower()
        {
            if (_towerToPlace == null || _towerManager == null) return;

            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity, boardMask) || !_isTowerSelected)
                return;

            playerActionsView.ShowRadiusAndRangeWidget();
            playerActionsView.MoveCircleToMouse(hit.point);

            if (!_towerManager.CanPlaceTower(_towerToPlace, hit.point))
            {
                playerActionsView.SetRadiusWidgetUnableToPlaceColor();
            }
            else
            {
                playerActionsView.SetRadiusWidgetDefaultColor();

                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    var command = new PlaceTowerCommand(_towerToPlace, hit.point);
                    CommandInvoker.Instance.ExecuteCommand(command);

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
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, towerMask))
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
            var capsule = _towerToPlace.GetComponentInChildren<CapsuleCollider>();
            if (capsule == null) return;

            _isTowerSelected = true;
            _towerRadius = capsule.radius;
            _towerRange = towerToPlace.GetBaseRange();

            playerActionsView.SetRadiusWidgetDefaultColor();
            playerActionsView.SetRadiusAndRangeWidgetSize(_towerRadius, _towerRange);
            playerActionsView.ShowRadiusAndRangeWidget();
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = _zAxis;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }

        public void OnSellTowerButtonClicked()
        {
            Debug.Log("Clicked Sell");

            AnimateButton(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform);

            if (_selectedTower != null)
            {
                Debug.Log("Selling tower: " + _selectedTower.name);
                var command = new SellTowerCommand(_selectedTower);
                CommandInvoker.Instance.ExecuteCommand(command);

                ClearSelect();
                _towerManager.UnselectTower();
            }
            else
            {
                Debug.LogWarning("No tower selected to sell.");
            }
        }

        public void OnUndoPressed()
        {
            AnimateButton(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform);
            
            CommandInvoker.Instance.Undo();
        }
        
        private void AnimateButton(Transform buttonTransform)
        {
            StartCoroutine(PunchScale(buttonTransform));
        }

        private IEnumerator PunchScale(Transform t)
        {
            Vector3 original = t.localScale;
            Vector3 target = original * 1.1f;
            float duration = 0.1f;
            float time = 0f;

            while (time < duration)
            {
                t.localScale = Vector3.Lerp(original, target, time / duration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            t.localScale = target;
            time = 0f;

            while (time < duration)
            {
                t.localScale = Vector3.Lerp(target, original, time / duration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            t.localScale = original;
        }

    }
}
