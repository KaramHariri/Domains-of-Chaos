using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private EnemyWave EnemyWaves;
    [SerializeField] private bool InfiniteSpawner;
    [SerializeField] private GameObject[] SpawnPoints;
    [SerializeField] private int CurrentWaveIndex = 0;
    private bool DoneSpawning = false;
    private bool StartedSpawning = false;
    GameObject WaveParentObject;
    public static float WaveDifficulty = 1f;
    private float DifficultyMultiplier = 1.05f;
    public void StartSpawningWaves()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        if(!StartedSpawning)
        {
            StartedSpawning = true;
            while (!DoneSpawning)
            {
                yield return new WaitUntil(() => { return GameManager.Instance.GetCurrentState() == GameManager.Instance.GameplayState; });
                if (CurrentWaveIndex >= EnemyWaves.Waves.Count && !InfiniteSpawner)
                {
                    yield break;
                }

                WaveParentObject = new GameObject("Wave" + (CurrentWaveIndex + 1));
                for (int i = 0; i < EnemyWaves.Waves[CurrentWaveIndex].NumberOfEnemiesInWave; i++)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(EnemyWaves.Waves[CurrentWaveIndex].TimeToNextEnemySpawn);
                }
                yield return new WaitForSeconds(EnemyWaves.TimeBetweenWaveSpawn);
                if (CurrentWaveIndex < EnemyWaves.Waves.Count - 1)
                {
                    CurrentWaveIndex++;
                }
                else if (CurrentWaveIndex >= EnemyWaves.Waves.Count && !InfiniteSpawner)
                {
                    DoneSpawning = true;
                }

                WaveDifficulty = WaveDifficulty * DifficultyMultiplier;
            }
        }
    }

    void SpawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, EnemyWaves.Waves[CurrentWaveIndex].EnemiesToSpawInWave.Length);
        int randomSpawnIndex = Random.Range(0, SpawnPoints.Length);
        GameObject enemyToSpawn = ObjectPooler.Instance.SpawnFromPool(EnemyWaves.Waves[CurrentWaveIndex].EnemiesToSpawInWave[randomEnemyIndex].name,
                                                                      SpawnPoints[randomSpawnIndex].transform.position,
                                                                      Quaternion.identity);
    }
}
