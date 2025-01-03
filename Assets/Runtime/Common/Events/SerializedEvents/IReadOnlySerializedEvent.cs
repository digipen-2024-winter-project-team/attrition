using System;

namespace Attrition.Common.Events.SerializedEvents
{
    public interface IReadOnlySerializedEvent
    {
        /// <summary>
        /// The C# event that gets invoked. This is called before UnityEvent by default.
        /// </summary>
        event Action Invoked;
    }
    
    public interface IReadOnlySerializedEvent<out T>
    {
        /// <summary>
        /// The C# event with a parameter of type <typeparamref name="T"/> that gets invoked. This is called before
        /// UnityEvent by default.
        /// </summary>
        event Action<T> Invoked;
    }
}
