using System;
using System.Collections.Generic;
using Attrition.Common.Picking.Strategies;

namespace Attrition.Common.Picking
{
    /// <summary>
    /// Represents a picker that uses a specific strategy to select items.
    /// </summary>
    /// <typeparam name="T">The type of items to pick from.</typeparam>
    public class Picker<T>
    {
        /// <summary>
        /// Gets the strategy used by the picker.
        /// </summary>
        public IPickStrategy<T> Strategy { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Picker{T}"/> class with the specified strategy.
        /// </summary>
        /// <param name="strategy">The picking strategy to use.</param>
        /// <exception cref="ArgumentNullException">Thrown when the strategy is null.</exception>
        public Picker(IPickStrategy<T> strategy)
        {
            this.Strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        /// <summary>
        /// Picks an item from the given collection using the configured strategy.
        /// </summary>
        /// <param name="items">The collection of items to pick from.</param>
        /// <returns>The selected item.</returns>
        public T PickFrom(IEnumerable<T> items)
        {
            return this.Strategy.Pick(items);
        }
    }
}
