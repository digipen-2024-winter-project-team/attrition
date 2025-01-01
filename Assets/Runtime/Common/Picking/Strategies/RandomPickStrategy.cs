using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Attrition.Common.Picking.Strategies
{
    /// <summary>
    /// Implements a strategy for randomly selecting an item from a collection.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public class RandomPickStrategy<T> : IPickStrategy<T>
    {
        private readonly ThreadLocal<Random> threadLocalRandom = new(() => new());

        /// <summary>
        /// Picks a random item from the given collection.
        /// </summary>
        /// <param name="items">The collection of items to pick from.</param>
        /// <returns>A randomly selected item from the collection.</returns>
        /// <exception cref="ArgumentException">Thrown when the collection is null or empty.</exception>
        public T Pick(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentException("The items collection must not be null.", nameof(items));
            }

            var list = items as IList<T> ?? items.ToList();
            if (list.Count == 0)
            {
                throw new ArgumentException("The items collection must not be empty.", nameof(items));
            }

            var index = this.threadLocalRandom.Value.Next(list.Count);
            return list[index];
        }
    }
}
