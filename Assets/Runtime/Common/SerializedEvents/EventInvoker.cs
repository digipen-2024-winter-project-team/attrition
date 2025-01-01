using System;

namespace Attrition.Common.SerializedEvents
{
    /// <summary>
    /// Static helper class for invoking events in a specified order.
    /// </summary>
    internal static class EventInvoker
    {
        /// <summary>
        /// Invokes three events (C#, Unity, ScriptableObject) in the specified order.
        /// </summary>
        /// <param name="order">The order in which the events are invoked.</param>
        /// <param name="cSharpEvent">The C# event to invoke.</param>
        /// <param name="unityEvent">The UnityEvent to invoke.</param>
        /// <param name="scriptableEvent">The ScriptableObject event to invoke.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided <paramref name="order"/> is not valid.</exception>
        internal static void InvokeEvents(
            EventInvocationOrder order,
            Action cSharpEvent,
            Action unityEvent,
            Action scriptableEvent)
        {
            switch (order)
            {
                case EventInvocationOrder.CSharpThenUnityThenScriptable:
                    cSharpEvent?.Invoke();
                    unityEvent?.Invoke();
                    scriptableEvent?.Invoke();
                    break;

                case EventInvocationOrder.CSharpThenScriptableThenUnity:
                    cSharpEvent?.Invoke();
                    scriptableEvent?.Invoke();
                    unityEvent?.Invoke();
                    break;

                case EventInvocationOrder.UnityThenCSharpThenScriptable:
                    unityEvent?.Invoke();
                    cSharpEvent?.Invoke();
                    scriptableEvent?.Invoke();
                    break;

                case EventInvocationOrder.UnityThenScriptableThenCSharp:
                    unityEvent?.Invoke();
                    scriptableEvent?.Invoke();
                    cSharpEvent?.Invoke();
                    break;

                case EventInvocationOrder.ScriptableThenCSharpThenUnity:
                    scriptableEvent?.Invoke();
                    cSharpEvent?.Invoke();
                    unityEvent?.Invoke();
                    break;

                case EventInvocationOrder.ScriptableThenUnityThenCSharp:
                    scriptableEvent?.Invoke();
                    unityEvent?.Invoke();
                    cSharpEvent?.Invoke();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(order), order, null);
            }
        }
        
        /// <summary>
        /// Invokes three generic events (C#, Unity, ScriptableObject) with an argument in the specified order.
        /// </summary>
        /// <typeparam name="T">The type of the argument passed to the events.</typeparam>
        /// <param name="value">The argument to pass to the events.</param>
        /// <param name="order">The order in which the events are invoked.</param>
        /// <param name="cSharpEvent">The C# event to invoke.</param>
        /// <param name="unityEvent">The UnityEvent to invoke.</param>
        /// <param name="scriptableEvent">The ScriptableObject event to invoke.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided <paramref name="order"/> is not valid.</exception>
        public static void InvokeEvents<T>(
            T value,
            EventInvocationOrder order,
            Action<T> cSharpEvent,
            Action<T> unityEvent,
            Action<T> scriptableEvent)
        {
            switch (order)
            {
                case EventInvocationOrder.CSharpThenUnityThenScriptable:
                    cSharpEvent?.Invoke(value);
                    unityEvent?.Invoke(value);
                    scriptableEvent?.Invoke(value);
                    break;

                case EventInvocationOrder.CSharpThenScriptableThenUnity:
                    cSharpEvent?.Invoke(value);
                    scriptableEvent?.Invoke(value);
                    unityEvent?.Invoke(value);
                    break;

                case EventInvocationOrder.UnityThenCSharpThenScriptable:
                    unityEvent?.Invoke(value);
                    cSharpEvent?.Invoke(value);
                    scriptableEvent?.Invoke(value);
                    break;

                case EventInvocationOrder.UnityThenScriptableThenCSharp:
                    unityEvent?.Invoke(value);
                    scriptableEvent?.Invoke(value);
                    cSharpEvent?.Invoke(value);
                    break;

                case EventInvocationOrder.ScriptableThenCSharpThenUnity:
                    scriptableEvent?.Invoke(value);
                    cSharpEvent?.Invoke(value);
                    unityEvent?.Invoke(value);
                    break;

                case EventInvocationOrder.ScriptableThenUnityThenCSharp:
                    scriptableEvent?.Invoke(value);
                    unityEvent?.Invoke(value);
                    cSharpEvent?.Invoke(value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(order), order, null);
            }
        }
    }
}
