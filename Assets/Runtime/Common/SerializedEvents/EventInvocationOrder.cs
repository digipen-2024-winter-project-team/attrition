namespace Attrition.Common.SerializedEvents
{
    /// <summary>
    /// Enum representing the order in which events are invoked for <see cref="SerializedEvent"/>.
    /// </summary>
    public enum EventInvocationOrder
    {
        /// <summary>
        /// Invoke events in the order: C# methods, then Unity Events, then ScriptableEvents.
        /// </summary>
        CSharpThenUnityThenScriptable,

        /// <summary>
        /// Invoke events in the order: C# methods, then ScriptableEvents, then Unity Events.
        /// </summary>
        CSharpThenScriptableThenUnity,

        /// <summary>
        /// Invoke events in the order: Unity Events, then C# methods, then ScriptableEvents.
        /// </summary>
        UnityThenCSharpThenScriptable,

        /// <summary>
        /// Invoke events in the order: Unity Events, then ScriptableEvents, then C# methods.
        /// </summary>
        UnityThenScriptableThenCSharp,

        /// <summary>
        /// Invoke events in the order: ScriptableEvents, then C# methods, then Unity Events.
        /// </summary>
        ScriptableThenCSharpThenUnity,

        /// <summary>
        /// Invoke events in the order: ScriptableEvents, then Unity Events, then C# methods.
        /// </summary>
        ScriptableThenUnityThenCSharp,
    }
}
