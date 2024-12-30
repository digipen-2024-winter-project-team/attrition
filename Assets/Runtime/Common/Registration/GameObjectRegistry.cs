using UnityEngine;

namespace Attrition.Common.Registration
{
    [CreateAssetMenu(menuName = "Scriptables/Registries/GameObject", fileName = "New GameObject Registry")]
    public class GameObjectRegistry : ScriptableRegistry<GameObject>
    {
    }
}
