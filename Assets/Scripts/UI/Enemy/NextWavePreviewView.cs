using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Enemy
{
    public class NextWavePreviewView : MonoBehaviour, INextWavePreviewView
    {
        [SerializeField] private GameObject textEntryPrefab;
        [SerializeField] private Transform contentParent;

        public void ShowWavePreview(List<EnemyWaveInfo> enemies)
        {
            foreach (Transform child in contentParent)
                Destroy(child.gameObject);

            foreach (var enemy in enemies)
            {
                var go = Instantiate(textEntryPrefab, contentParent);
                var text = go.GetComponentInChildren<Text>();
                text.text = $"{enemy.enemyName} x{enemy.count}";
            }
        }
    }
}