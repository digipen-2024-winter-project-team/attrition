using System;
using System.Collections.Generic;
using System.Linq;
using Attrition.Common.Picking.Strategies;

namespace Attrition.Common.Picking.Decorators
{
    /// <summary>
    /// Decorator that ensures blacklisted items are not picked.
    /// </summary>
    /// <typeparam name="T">The type of items to pick from.</typeparam>
    public class BlacklistPickStrategyDecorator<T> : PickStrategyDecorator<T>
    {
        private readonly HashSet<T> blacklist;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlacklistPickStrategyDecorator{T}"/> class.
        /// </summary>
        /// <param name="innerStrategy">The inner strategy to decorate.</param>
        /// <param name="blacklist">The collection of blacklisted items.</param>
        public BlacklistPickStrategyDecorator(IPickStrategy<T> innerStrategy, IEnumerable<T> blacklist)
            : base(innerStrategy)
        {
            if (blacklist == null)
            {
                throw new ArgumentNullException(nameof(blacklist));
            }

            this.blacklist = new HashSet<T>(blacklist);
        }

        /// <summary>
        /// Picks an item from the collection, ensuring it is not in the blacklist.
        /// </summary>
        /// <param name="items">The collection of items to pick from.</param>
        /// <returns>The selected item.</returns>
        /// <exception cref="ArgumentException">Thrown when the collection is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no non-blacklisted items are available.</exception>
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

            // Filter the items against the blacklist
            var filtered = list.Where(item => !this.blacklist.Contains(item)).ToList();

            if (filtered.Count == 0)
            {
                throw new InvalidOperationException("No non-blacklisted items are available to pick.");
            }

            // Delegate picking to the inner strategy
            return this.InnerStrategy.Pick(filtered);
        }
    }
}
