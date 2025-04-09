using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WaveUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text waveText; 

        private void Awake()
        {
            UpdateWaveText(1);
        }

        public void UpdateWaveText(int waveNumber)
        {
            waveText.text = "Wave: " + waveNumber;
        }
    }
}