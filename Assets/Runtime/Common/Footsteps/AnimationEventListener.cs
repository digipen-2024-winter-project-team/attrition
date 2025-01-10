using Attrition.Common.Events.SerializedEvents;
using UnityEngine;

namespace Attrition.Common.Footsteps
{
    // At some point I'd have a crack at turning both this and the FootstepRelay into general purpose classes that can
    // listen for any animation event, not just footsteps.
    public class AnimationEventListener : MonoBehaviour
    {
        [SerializeField]
        private SerializedEvent onFootL;
        [SerializeField]
        private SerializedEvent onFootR;
        
        public void OnFootL()
        {
            this.onFootL.Invoke();
        }

        public void OnFootR()
        {
            this.onFootR.Invoke();
        }
    }
}
