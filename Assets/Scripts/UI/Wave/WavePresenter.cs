namespace Wave
{
    public class WavePresenter
    {
        private WaveManager _model;
        private IWaveView _view;

        public WavePresenter(WaveManager model, IWaveView view)
        {
            _model = model;
            _view = view;
            
            _view.UpdateWaveCounter(_model.GetCurrentWave());
            
            WaveManager.OnWaveChanged += HandleWaveChanged;
        }

        private void HandleWaveChanged(int wave)
        {
            _view.UpdateWaveCounter(wave);
        }
    }
}