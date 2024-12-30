using System;
using System.Collections.Generic;
using Attrition.Common.Picking.Strategies;

namespace Attrition.Common.Picking.Decorators
{
    /// <summary>
    /// Base class for decorators that add functionality to pick strategies.
    /// </summary>
    /// <typeparam name="T">The type of items to pick from.</typeparam>
    public class PickStrategyDecorator<T> : IPickStrategy<T>
    {
        /// <summary>
        /// Gets the inner strategy being decorated.
        /// </summary>
        protected readonly IPickStrategy<T> InnerStrategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="PickStrategyDecorator{T}"/> class.
        /// </summary>
        /// <param name="innerStrategy">The strategy to decorate.</param>
        /// <exception cref="ArgumentNullException">Thrown when the inner strategy is null.</exception>
        public PickStrategyDecorator(IPickStrategy<T> innerStrategy)
        {
            this.InnerStrategy = innerStrategy ?? throw new ArgumentNullException(nameof(innerStrategy));
        }

        /// <summary>
        /// Picks an item using the decorated strategy.
        /// </summary>
        /// <param name="items">The collection of items to pick from.</param>
        /// <returns>The selected item.</returns>
        public virtual T Pick(IEnumerable<T> items)
        {
            return this.InnerStrategy.Pick(items);
        }
    }
}
