using System;
using TMPro;
using Tower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class TowerMenuController : MonoBehaviour
    {
        [SerializeField] private TMP_Text shootingTowerCostText; 
        [SerializeField] private TMP_Text slowingTowerCostText;
        
        [SerializeField] private TowerManager towerManager;
        
        private GameObject _towerToPlace;
        private GameObject _innerRadius;
        private GameObject _outerRadius;
        private Vector3 _mousePosition;
        private float _zAxis = 0.2f;
        private bool _isTowerSelected = false;
        private float _towerRadius;
        private int _towerRange;
        private LineRenderer _lineRenderer;
        private Renderer _rend;
        private int _circleSegments = 100;
        
        [Header("Tower placing widgets")]
        [SerializeField] private GameObject radiusIndicator;
        [SerializeField] private Color outerCircleColor;
        [SerializeField] private Color innerCircleColor;
        
        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (_towerToPlace != null)
            {
               
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit,
                        Mathf.Infinity))
                {
                    MoveCircleToMouse(hit.point);
                    if (Input.GetMouseButtonDown(0))
                    {
                        towerManager.PlaceTower(_towerToPlace, hit.point);
                        _isTowerSelected = false;
                    }
                }
            }
        }
        public void UpdateShootingTowerCost(int cost)
        {
            shootingTowerCostText.text = cost.ToString() + "$";
        }

        public void UpdateSlowingTowerCost(int cost)
        {
            slowingTowerCostText.text = cost.ToString() + "$";
        }

        public void SelectTowerToPlace(GameObject gameObjectToPlace)
        {
            this._towerToPlace = gameObjectToPlace;
            CapsuleCollider capsuleCollider = _towerToPlace.GetComponentInChildren<CapsuleCollider>();
            if (capsuleCollider != null)
            {
                Debug.Log("Rendering tower");
                _towerRadius = capsuleCollider.radius;
                _towerRange = gameObjectToPlace.GetComponent<BaseTower>().GetRange();
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = _zAxis;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePosition);
                CreateRadiusAndRangeWidget(worldPos);
                CreateRadiusAndRangeWidget(worldPos);
            }
            else
            {
                Debug.Log("No rendering tower");
            }
            _isTowerSelected = true;
        }

        private void MoveCircleToMouse(Vector3 position)
        {
            if (_outerRadius != null && _innerRadius != null)
            {
                _outerRadius.transform.position = new Vector3(position.x, -0.49f, position.z);
                _innerRadius.transform.position = new Vector3(position.x, -0.48f, position.z);
            }
            
        }

        private void CreateRadiusAndRangeWidget(Vector3 position)
        {
           if (_innerRadius != null) Destroy(_innerRadius);
           if (_outerRadius != null) Destroy(_outerRadius);
           
           _innerRadius = Instantiate(radiusIndicator, position, Quaternion.identity);
           _outerRadius = Instantiate(radiusIndicator, position, Quaternion.identity);
           _innerRadius.transform.localScale = new Vector3(_towerRadius, 0, _towerRadius);
           _outerRadius.transform.localScale = new Vector3(_towerRange, 0, _towerRange);
           _innerRadius.GetComponent<Renderer>().material.color = innerCircleColor;
           _outerRadius.GetComponent<Renderer>().material.color = outerCircleColor;
        }
    }
}