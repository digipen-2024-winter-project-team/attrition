using System;
using UnityEngine;

namespace Attrition.Common.Events.ScriptableEvents
{
    [CreateAssetMenu(menuName = "Scriptables/Events/Scriptable Event")]
    public class ScriptableEvent : ScriptableObject
    {
        public event Action Invoked;
        
        public void Invoke()
        {
            this.Invoked?.Invoke();
        }
    }
}
