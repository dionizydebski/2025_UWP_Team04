﻿using System.Collections.Generic;
using Wave;

namespace UI.Enemy
{
    public class NextWavePresenter
    {
        private readonly WaveManager _waveManager;
        private readonly INextWavePreviewView _view;

        public NextWavePresenter(WaveManager waveManager, INextWavePreviewView view)
        {
            _waveManager = waveManager;
            _view = view;

            WaveManager.OnWavePreviewChanged += UpdateView;
            UpdateView();
        }

        public void UpdateView()
        {
            List<EnemyWaveInfo> nextWave = _waveManager.GetUpcomingWavePreview();
            _view.ShowWavePreview(nextWave);
        }
    }
}