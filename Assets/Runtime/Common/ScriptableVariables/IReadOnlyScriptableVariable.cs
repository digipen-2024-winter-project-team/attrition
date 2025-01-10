using System;

namespace Attrition.Common.ScriptableVariables
{
    public interface IReadOnlyScriptableVariable<TValue>
    {
        /// <summary>
        /// Gets or sets the current value of the variable.
        /// Setting the value will trigger the change notification process.
        /// </summary>
        public TValue Value { get; }

        /// <summary>
        /// Event triggered before the value of the variable changes.
        /// Handlers receive a <see cref="ValueChangeArgs{TValue}"/> object describing the change.
        /// </summary>
        public event Action<ValueChangeArgs<TValue>> ValueChanging;

        /// <summary>
        /// Event triggered after the value of the variable has changed.
        /// Handlers receive a <see cref="ValueChangeArgs{TValue}"/> object describing the change.
        /// </summary>
        public event Action<ValueChangeArgs<TValue>> ValueChanged;
    }
}
