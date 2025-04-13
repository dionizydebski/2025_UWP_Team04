using UnityEngine;
using UnityEngine.UI;

namespace Wave
{
    public class WaveCounterView : MonoBehaviour, IWaveView
    {
        [SerializeField] private Text waveText;

        private WavePresenter _presenter;

        void Start()
        {
            _presenter = new WavePresenter(WaveManager.Instance, this);
        }

        public void UpdateWaveCounter(int wave)
        {
            waveText.text = $"Wave: {wave}";
        }
    }
}