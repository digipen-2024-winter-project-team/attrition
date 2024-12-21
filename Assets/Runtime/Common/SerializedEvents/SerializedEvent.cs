using System;
using UnityEngine;
using UnityEngine.Events;

namespace Attrition.Common.SerializedEvents
{
    [Serializable]
    public class SerializedEvent
    {
        [SerializeField]
        private UnityEvent raised;

        public event Action Raised = () => { };
        
        public void Raise()
        {
            this.Raised?.Invoke();
            this.raised.Invoke();
        }
    }
    
    [Serializable]
    public class SerializedEvent<T>
    {
        [SerializeField]
        private UnityEvent<T> raised;

        public event Action<T> Raised = (args) => { };
        
        public void Raise(T value)
        {
            this.Raised?.Invoke(value);
            this.raised.Invoke(value);
        }
    }
}
