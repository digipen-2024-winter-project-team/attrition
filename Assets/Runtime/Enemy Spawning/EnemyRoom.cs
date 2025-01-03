using System.Collections.Generic;
using UnityEngine;

namespace Attrition.EnemySpawning
{
    public class EnemyRoom : MonoBehaviour
    {
        [SerializeField] private List<EnemySpawnData> enemies;

        public List<EnemySpawnData> Enemies => enemies;
        public EnemySpawnPoint[] SpawnPoints => GetComponentsInChildren<EnemySpawnPoint>();
    }
}
