using UnityEngine;
using UnityEngine.EventSystems;

namespace Attrition.UI
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
