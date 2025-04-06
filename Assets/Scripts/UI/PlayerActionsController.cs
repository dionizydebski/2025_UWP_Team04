using System;
using Tower;
using UnityEngine;

namespace UI
{
    public class PlayerActionsController : MonoBehaviour
    {
        [SerializeField] private TowerManager towerManager;
        [SerializeField] private string indicatorName;
        
        private GameObject _towerToPlace;
        private GameObject _innerRadius;
        private GameObject _outerRadius;
        private Vector3 _mousePosition;
        private float _zAxis = 0.2f;
        private bool _isTowerSelected;
        private float _towerRadius;
        private int _towerRange;
        private Renderer _rend;
        
        [Header("Tower placing widgets")]
        [SerializeField] private GameObject radiusIndicator;
        [SerializeField] private Color outerCircleColorDefault;
        [SerializeField] private Color innerCircleColorDefault;
        [SerializeField] private Color outerCircleColorCantPlace;
        [SerializeField] private Color innerCircleColorCantPlace;
        
        [SerializeField] private LayerMask boardMask;
        [SerializeField] private LayerMask towerMask;
        private GameObject selectedTower;
        private Renderer selectedTowerRenderer;
        private Transform rangeIndicator;


        private void Awake()
        {
            Cursor.visible = true;
        }

        private void Update()
        {
            if (_towerToPlace)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit,
                        Mathf.Infinity, boardMask) && _isTowerSelected)
                {
                    MoveCircleToMouse(hit.point);
                    if (!towerManager.CanPlaceTower(_towerToPlace, hit.point))
                    {
                        _innerRadius.GetComponent<Renderer>().material.color = innerCircleColorCantPlace;
                        _outerRadius.GetComponent<Renderer>().material.color = outerCircleColorCantPlace;
                    }
                    else
                    {
                        _innerRadius.GetComponent<Renderer>().material.color = innerCircleColorDefault;
                        _outerRadius.GetComponent<Renderer>().material.color = outerCircleColorDefault;
                        if (Input.GetMouseButtonDown(0))
                        {
                            towerManager.PlaceTower(_towerToPlace, hit.point);
                            _isTowerSelected = false;
                            _towerToPlace = null;
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Tried to select a tower.");
                SelectTower();
            }

            if (!_isTowerSelected)
            {
                if (_outerRadius) Destroy(_outerRadius);
                if (_innerRadius) Destroy(_innerRadius);
            }
        }

        private void SelectTower()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity,towerMask))
            {
                GameObject hitObject = hit.collider.gameObject.transform.root.gameObject;
                
                Debug.Log(hitObject.name);

                if (selectedTower != hitObject)
                {
                    ClearSelect();
                    
                    selectedTower = hitObject;
                    selectedTowerRenderer = hitObject.GetComponent<Renderer>();

                    if (selectedTowerRenderer)
                    {
                        //TODO: tower highlighting
                    }
                    
                    rangeIndicator = selectedTower.transform.Find(indicatorName);
                    if (rangeIndicator)
                    {
                        Debug.Log("Range indicator found");
                        BaseTower tower = selectedTower.GetComponent<BaseTower>();
                        rangeIndicator.localScale = new Vector3(tower.GetRange(), 0.01f, tower.GetRange());
                        rangeIndicator.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                ClearSelect();
            }
        }

        private void ClearSelect()
        {
            if (selectedTower)
            {
                if (selectedTowerRenderer)
                {
                    //TODO:Clear highlighting
                }

                if (rangeIndicator)
                {
                    rangeIndicator.gameObject.SetActive(false);
                }
            }
            selectedTower = null;
            selectedTowerRenderer = null;
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
           _innerRadius.GetComponent<Renderer>().material.color = innerCircleColorDefault;
           _outerRadius.GetComponent<Renderer>().material.color = outerCircleColorDefault;
        }
    }
}