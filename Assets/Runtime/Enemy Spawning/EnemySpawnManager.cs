using UnityEngine;
using Random = UnityEngine.Random;

namespace Attrition.EnemySpawning
{
    public class EnemySpawnManager : MonoBehaviour
    {
        private void Start()
        {
            var rooms = GetComponentsInChildren<EnemyRoom>();

            foreach (var room in rooms)
            {
                if (room.Enemies.Count == 0)
                {
                    continue;
                }
                
                foreach (var spawnPoint in room.SpawnPoints)
                {
                    var enemy = room.Enemies[Random.Range(0, room.Enemies.Count)];

                    Instantiate(enemy.Prefab, spawnPoint.transform);
                }
            }
        }
    }
}
