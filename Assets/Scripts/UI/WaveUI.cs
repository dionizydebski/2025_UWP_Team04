using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private Text waveText; 

    private void Start()
    {
        UpdateWaveText(1);
    }

    public void UpdateWaveText(int waveNumber)
    {
        waveText.text = "Wave: " + waveNumber;
    }
}