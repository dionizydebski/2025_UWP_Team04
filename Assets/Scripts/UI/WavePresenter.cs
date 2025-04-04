using Wave;

namespace Core
{
    using System.Collections;
using UnityEngine;

public class WavePresenter
{
    private readonly WaveModel model;
    private readonly WaveUI view;
    private readonly WaveManager waveManager;
    
    private float timeBetweenWaves;
    private float difficultyFactor;
    private int baseEnemies;

    public bool IsSpawning { get; private set; }
    public int EnemiesLeftToSpawn { get; private set; }
    public int EnemiesAlive { get; private set; }

    private float spawnTimer;

    public WavePresenter(WaveModel model, WaveUI view, WaveManager manager,
                         float timeBetweenWaves, float difficultyFactor, int baseEnemies)
    {
        this.model = model;
        this.view = view;
        this.waveManager = manager;

        this.timeBetweenWaves = timeBetweenWaves;
        this.difficultyFactor = difficultyFactor;
        this.baseEnemies = baseEnemies;

        model.OnWaveChanged += view.UpdateWaveText;
    }

    public IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        IsSpawning = true;
        EnemiesLeftToSpawn = EnemiesPerWave();
        EnemiesAlive = 0;

        view.UpdateWaveText(model.CurrentWave);
    }

    public void Update(float deltaTime)
    {
        if (!IsSpawning) return;

        spawnTimer += deltaTime;

        if (spawnTimer >= (1f / waveManager.enemiesPerSecond) && EnemiesLeftToSpawn > 0)
        {
            waveManager.SpawnEnemy();
            EnemiesLeftToSpawn--;
            EnemiesAlive++;
            spawnTimer = 0f;
        }

        if (EnemiesLeftToSpawn == 0 && EnemiesAlive == 0)
        {
            EndWave();
        }
    }

    public void EnemyDestroyed()
    {
        EnemiesAlive--;
    }

    private void EndWave()
    {
        IsSpawning = false;
        spawnTimer = 0f;
        model.NextWave();
        waveManager.StartCoroutine(StartWave());
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(model.CurrentWave, difficultyFactor));
    }
}

}