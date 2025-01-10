using UnityEngine;

namespace Attrition.Common.Footsteps
{
    public class FootstepsRelay : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        
        public void OnFootL()
        {
            this.Relay(this.target, nameof(this.OnFootL));
        }

        public void OnFootR()
        {
            this.Relay(this.target, nameof(this.OnFootR));
        }

        private void Relay(Transform target, string message)
        {
            target.SendMessage(message, SendMessageOptions.DontRequireReceiver);
        }
    }
}
