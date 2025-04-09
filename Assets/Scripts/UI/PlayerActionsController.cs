﻿using Tower;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class PlayerActionsController : MonoBehaviour
    {
        private TowerManager towerManager;
        private GameObject _towerToPlace;

        private Vector3 _mousePosition;
        private float _zAxis = 0.2f;
        private bool _isTowerSelected;
        private float _towerRadius;
        private int _towerRange;
        private Renderer _rend;

        [SerializeField] private LayerMask boardMask;
        [SerializeField] private LayerMask towerMask;
        [SerializeField] private LayerMask uiMask;

        private GameObject selectedTower;
 
        [SerializeField] private PlayerActionsView playerActionsView;

        private void Awake()
        {
            Cursor.visible = true;
            towerManager = TowerManager.Instance;
            playerActionsView.CreateRadiusAndRangeWidget(GetMouseWorldPosition(), 1, 1);
        }

        private void Update()
        {
            if (_towerToPlace)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit,
                        Mathf.Infinity, boardMask) && _isTowerSelected)
                {
                    playerActionsView.ShowRadiusAndRangeWidget();
                    playerActionsView.MoveCircleToMouse(hit.point);
                    if (!towerManager.CanPlaceTower(_towerToPlace, hit.point))
                    {
                        Debug.Log("Can't place tower");
                        playerActionsView.SetRadiusWidgetUnableToPlaceColor();
                    }
                    else
                    {
                        playerActionsView.SetRadiusWidgetDefaultColor();
                        if (Input.GetMouseButtonDown(0))
                        {
                            if (EventSystem.current.IsPointerOverGameObject()) return;
                            towerManager.PlaceTower(_towerToPlace, hit.point);
                            _isTowerSelected = false;
                            _towerToPlace = null;
                        }
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        playerActionsView.HideRadiusAndRangeWidget();
                        _isTowerSelected = false;
                        _towerToPlace = null;
                    }
                }
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

        private void SelectTower()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, towerMask))
            {
                GameObject hitObject = hit.collider.gameObject.transform.root.gameObject;

                if (selectedTower != hitObject)
                {
                    ClearSelect();

                    selectedTower = hitObject;

                    playerActionsView.ShowTowerRangeIndicator(selectedTower);

                    towerManager.SelectTower(selectedTower);
                }
            }
            else
            {
                Debug.Log("Unselect");
                ClearSelect();
                towerManager.UnselectTower();
            }
        }

        

        private void ClearSelect()
        {
            if (selectedTower)
            {
                playerActionsView.HideTowerRangeIndicator();
            }
            selectedTower = null;
        }

        public void SelectTowerToPlace(GameObject gameObjectToPlace)
        { 
            _towerToPlace = gameObjectToPlace;
            CapsuleCollider capsuleCollider = _towerToPlace.GetComponentInChildren<CapsuleCollider>();

            if (capsuleCollider == null) return;
            _isTowerSelected = true;
            _towerRadius = capsuleCollider.radius;
            _towerRange = gameObjectToPlace.GetComponent<BaseTower>().GetRange();

            playerActionsView.SetRadiusWidgetDefaultColor();
            playerActionsView.SetRadiusAndRangeWidgetSize(_towerRadius, _towerRange);
            playerActionsView.ShowRadiusAndRangeWidget();
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = _zAxis;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePosition);
            return worldPos;
        }
    }
}