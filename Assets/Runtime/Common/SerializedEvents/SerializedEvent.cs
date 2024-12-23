using System;
using UnityEngine;
using UnityEngine.Events;

namespace Attrition.Common.SerializedEvents
{
    /// <summary>
    /// A serializable event wrapper that supports custom invocation order.
    /// </summary>
    [Serializable]
    public class SerializedEvent : IReadOnlySerializedEvent
    {
        /// <summary>
        /// The UnityEvent that gets invoked.
        /// </summary>
        [SerializeField]
        private UnityEvent invoked;

        /// <summary>
        /// The C# event that gets invoked. This is called before UnityEvent by default.
        /// </summary>
        public event Action Invoked = () => { };

        /// <summary>
        /// Invokes the event in the specified order.
        /// </summary>
        /// <param name="order">The order in which the events are invoked. Defaults to
        /// <see cref="EventInvocationOrder.CSharpThenUnityThenScriptable"/>.</param>
        public void Invoke(EventInvocationOrder order = EventInvocationOrder.CSharpThenUnityThenScriptable)
        {
            EventInvoker.InvokeEvents(
                order,
                this.Invoked,
                this.invoked.Invoke,
                null
            );
        }
    }
    
    /// <summary>
    /// A generic, serializable event wrapper that supports custom invocation order.
    /// </summary>
    /// <typeparam name="T">The type of argument passed with the event.</typeparam>
    [Serializable]
    public class SerializedEvent<T> : IReadOnlySerializedEvent<T>
    {
        /// <summary>
        /// The UnityEvent with a parameter of type <typeparamref name="T"/> that gets invoked.
        /// </summary>
        [SerializeField]
        private UnityEvent<T> invoked;

        /// <summary>
        /// The C# event with a parameter of type <typeparamref name="T"/> that gets invoked. This is called before
        /// UnityEvent by default.
        /// </summary>
        public event Action<T> Invoked = (args) => { };

        /// <summary>
        /// Invokes the event with the specified argument in the specified order.
        /// </summary>
        /// <param name="value">The argument to pass to the event.</param>
        /// <param name="order">The order in which the events are invoked. Defaults to
        /// <see cref="EventInvocationOrder.CSharpThenUnityThenScriptable"/>.</param>
        public void Invoke(T value, EventInvocationOrder order = EventInvocationOrder.CSharpThenUnityThenScriptable)
        {
            EventInvoker.InvokeEvents(
                value,
                order,
                this.Invoked,
                this.invoked.Invoke,
                null
            );
        }
    }
}
