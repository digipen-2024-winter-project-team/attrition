using UnityEngine;
using UnityEngine.EventSystems;

namespace Attrition.Character_Selection
{
    public class SelectOnEnable : MonoBehaviour
    {
        [SerializeField]
        private GameObject target;
        
        private void OnEnable()
        {
            this.SelectTarget();
        }

        private void SelectTarget()
        {
            EventSystem.current.SetSelectedGameObject(this.target);
        }
    }
}
