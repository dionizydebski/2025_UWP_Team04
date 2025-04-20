using Tower;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class PlayerActionsView : MonoBehaviour
    {
        [Header("Tower placing widgets")] 
        [SerializeField] private GameObject radiusIndicator;
        private GameObject _innerRadius;
        private GameObject _outerRadius;
        
        [Header("Tower placing widgets colors")]
        [SerializeField] private Color outerCircleColorDefault;
        [SerializeField] private Color innerCircleColorDefault;
        [SerializeField] private Color outerCircleColorCantPlace;
        [SerializeField] private Color innerCircleColorCantPlace;
        
        [Header("Tower range indicator")] 
        [SerializeField] private string indicatorName;
        
        [Header("References")]
        [SerializeField] private PlayerActionsController playerActionsController;
        
        private Renderer selectedTowerRenderer;
        private Renderer _innerRadiusRenderer;
        private Renderer _outerRadiusRenderer;
        
        private Transform rangeIndicator;

        public void CreateRadiusAndRangeWidget(Vector3 position, float towerRadius, float towerRange)
        {
            Debug.Log(position);
            if (_innerRadius != null) Destroy(_innerRadius);
            if (_outerRadius != null) Destroy(_outerRadius);

            _innerRadius = Instantiate(radiusIndicator, position, Quaternion.identity);
            _outerRadius = Instantiate(radiusIndicator, position, Quaternion.identity);
            _innerRadius.transform.localScale = new Vector3(towerRadius, 0, towerRadius);
            _outerRadius.transform.localScale = new Vector3(towerRange, 0, towerRange);
            _innerRadius.SetActive(false);
            _outerRadius.SetActive(false);
            _innerRadiusRenderer = _innerRadius.GetComponent<Renderer>();
            _outerRadiusRenderer = _outerRadius.GetComponent<Renderer>();
            
            SetRadiusWidgetDefaultColor();
        }
        
        public void SetRadiusWidgetDefaultColor()
        {
            _innerRadiusRenderer.material.color = innerCircleColorDefault;
            _outerRadiusRenderer.material.color = outerCircleColorDefault;
        }

        public void SetRadiusWidgetUnableToPlaceColor()
        {
            _innerRadiusRenderer.material.color = innerCircleColorCantPlace;
            _outerRadiusRenderer.material.color = outerCircleColorCantPlace;
        }

        public void ShowRadiusAndRangeWidget()
        {
            if (!_innerRadius || !_outerRadius) return;
            _innerRadius.SetActive(true);
            _outerRadius.SetActive(true);
        }

        public void HideRadiusAndRangeWidget()
        {
            if (!_innerRadius || !_outerRadius) return;
            _innerRadius.SetActive(false);
            _outerRadius.SetActive(false);
        }

        public void SetRadiusAndRangeWidgetSize(float towerRadius, float towerRange)
        {
            _innerRadius.transform.localScale = new Vector3(towerRadius, 0, towerRadius);
            _outerRadius.transform.localScale = new Vector3(towerRange, 0, towerRange);
        }
        
        public void MoveCircleToMouse(Vector3 position)
        {
            if (_outerRadius != null && _innerRadius != null)
            {
                _outerRadius.transform.position = new Vector3(position.x, -0.49f, position.z);
                _innerRadius.transform.position = new Vector3(position.x, -0.48f, position.z);
            }
        }
        
        public void ShowTowerRangeIndicator(GameObject selectedTower)
        {
            rangeIndicator = selectedTower.transform.Find(indicatorName);
            if (rangeIndicator)
            {
                BaseTower tower = selectedTower.GetComponent<BaseTower>();
                rangeIndicator.localScale = new Vector3(tower.GetCurrentRange(), 0.01f, tower.GetCurrentRange());
                rangeIndicator.gameObject.SetActive(true);
            }
        }

        public void HideTowerRangeIndicator()
        {
            rangeIndicator.gameObject.SetActive(false);
        }
    }
}