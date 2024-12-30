using System;
using UnityEngine;

namespace Attrition.Common.ScriptableVariables
{
    /// <summary>
    /// An abstract base class for ScriptableObjects that represent a variable of type <typeparamref name="TValue"/>.
    /// This class provides functionality for value storage, notification of value changes, and events for monitoring changes.
    /// </summary>
    /// <typeparam name="TValue">The type of the value stored by the variable.</typeparam>
    public abstract class ScriptableVariable<TValue> : ScriptableObject, IReadOnlyScriptableVariable<TValue>
    {
        /// <summary>
        /// The stored value of the variable.
        /// </summary>
        [SerializeField]
        private TValue value;

        /// <summary>
        /// Gets or sets the current value of the variable.
        /// Setting the value will trigger the change notification process.
        /// </summary>
        public TValue Value
        {
            get => this.value;
            set => this.SetValueWithNotify(value);
        }

        /// <summary>
        /// Event triggered before the value of the variable changes.
        /// Handlers receive a <see cref="ValueChangeArgs{TValue}"/> object describing the change.
        /// </summary>
        public event Action<ValueChangeArgs<TValue>> ValueChanging = args => { };

        /// <summary>
        /// Event triggered after the value of the variable has changed.
        /// Handlers receive a <see cref="ValueChangeArgs{TValue}"/> object describing the change.
        /// </summary>
        public event Action<ValueChangeArgs<TValue>> ValueChanged = args => { };

        /// <summary>
        /// Notifies listeners of the <see cref="ValueChanging"/> and <see cref="ValueChanged"/> events with the specified arguments.
        /// If no arguments are provided, default arguments are generated with the current value as both "From" and "To".
        /// </summary>
        /// <param name="args">Optional. The arguments describing the value change.</param>
        public void NotifyValueChanged(ValueChangeArgs<TValue> args = null)
        {
            args ??= new()
            {
                From = this.value,
                To = this.value,
            };

            this.ValueChanging?.Invoke(args);
            this.ValueChanged?.Invoke(args);
        }

        /// <summary>
        /// Sets the value of the variable without triggering the change notification process.
        /// </summary>
        /// <param name="value">The new value to set.</param>
        public void SetValueWithoutNotify(TValue value)
        {
            this.value = value;
        }

        /// <summary>
        /// Sets the value of the variable while triggering the change notification process.
        /// </summary>
        /// <param name="value">The new value to set.</param>
        private void SetValueWithNotify(TValue value)
        {
            var args = new ValueChangeArgs<TValue>()
            {
                From = this.value,
                To = value,
            };

            this.ValueChanging?.Invoke(args);
            this.value = value;
            this.ValueChanged?.Invoke(args);
        }
    }
}
