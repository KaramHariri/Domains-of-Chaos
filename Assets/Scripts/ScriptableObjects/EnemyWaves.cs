using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaves", menuName = "Scriptable Objects/EnemyWaves")]
public class EnemyWave : ScriptableObject
{
    public List<Wave> Waves;
    public float TimeBetweenWaveSpawn;
}

[System.Serializable]
public class Wave
{
    public GameObject[] EnemiesToSpawInWave;
    public int NumberOfEnemiesInWave;
    public float TimeToNextEnemySpawn;
}