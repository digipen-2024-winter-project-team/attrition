using System;
using System.Collections.Generic;
using System.Linq;
using Attrition.Common.Picking.Strategies;

namespace Attrition.Common.Picking.Decorators
{
    /// <summary>
    /// Decorator that ensures only whitelisted items are picked.
    /// </summary>
    /// <typeparam name="T">The type of items to pick from.</typeparam>
    public class WhitelistPickStrategyDecorator<T> : PickStrategyDecorator<T>
    {
        private readonly HashSet<T> whitelist;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhitelistPickStrategyDecorator{T}"/> class.
        /// </summary>
        /// <param name="innerStrategy">The inner strategy to decorate.</param>
        /// <param name="whitelist">The collection of whitelisted items.</param>
        public WhitelistPickStrategyDecorator(IPickStrategy<T> innerStrategy, IEnumerable<T> whitelist)
            : base(innerStrategy)
        {
            if (whitelist == null)
            {
                throw new ArgumentNullException(nameof(whitelist));
            }

            this.whitelist = new(whitelist);
        }

        /// <summary>
        /// Picks an item from the collection, ensuring it is in the whitelist.
        /// </summary>
        /// <param name="items">The collection of items to pick from.</param>
        /// <returns>The selected item.</returns>
        /// <exception cref="ArgumentException">Thrown when the collection is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no whitelisted items are available.</exception>
        public override T Pick(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentException("The items collection must not be null.", nameof(items));
            }

            // Convert to a list to ensure materialization and allow multiple iterations
            var list = items.ToList();

            if (list.Count == 0)
            {
                throw new ArgumentException("The items collection must not be empty.", nameof(items));
            }

            // Filter the items against the whitelist
            var filtered = list.Where(this.whitelist.Contains).ToList();

            if (filtered.Count == 0)
            {
                throw new InvalidOperationException("No whitelisted items are available to pick.");
            }

            // Delegate picking to the inner strategy
            return this.InnerStrategy.Pick(filtered);
        }
    }
}
