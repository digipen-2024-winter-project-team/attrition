using UnityEngine;
using UnityEngine.Events;

namespace Attrition.Common.Events.ScriptableEvents
{
    public class ScriptableEventListener : MonoBehaviour
    {
        [SerializeField]
        private ScriptableEvent @event;
        [SerializeField]
        private UnityEvent response;

        private void OnEnable()
        {
            this.@event.Invoked += this.OnInvoked;
        }

        private void OnDisable()
        {
            this.@event.Invoked -= this.OnInvoked;
        }

        private void OnInvoked()
        {
            this.response.Invoke();
        }
    }
}
