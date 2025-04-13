using System.Collections.Generic;

namespace UI.Enemy
{
    public interface INextWavePreviewView
    {
        void ShowWavePreview(List<EnemyWaveInfo> enemies);
    }
}