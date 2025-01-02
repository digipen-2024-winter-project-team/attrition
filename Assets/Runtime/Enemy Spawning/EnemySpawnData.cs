using UnityEngine;

namespace Attrition
{
    [CreateAssetMenu(menuName = "Attrition/Enemies/Enemy Spawn Data")]
    public class EnemySpawnData : ScriptableObject
    {
        [SerializeField] private GameObject enemyPrefab;

        public GameObject Prefab => enemyPrefab;
    }
}
