using System;
using System.Collections.Generic;
using System.Linq;

namespace Attrition.Common.Picking.Strategies
{
    /// <summary>
    /// Implements a round-robin selection strategy.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public class RoundRobinPickStrategy<T> : IPickStrategy<T>
    {
        private int currentIndex = -1;
        private readonly object lockObj = new object();

        /// <summary>
        /// Picks the next item in a round-robin fashion from the given collection.
        /// </summary>
        /// <param name="items">The collection of items to pick from.</param>
        /// <returns>The next item in the collection.</returns>
        /// <exception cref="ArgumentException">Thrown when the collection is null or empty.</exception>
        public T Pick(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentException("The items collection must not be null.", nameof(items));
            }

            var itemList = items as IList<T> ?? items.ToList();
            if (itemList.Count == 0)
            {
                throw new ArgumentException("The items collection must not be empty.", nameof(items));
            }

            lock (this.lockObj)
            {
                this.currentIndex = (this.currentIndex + 1) % itemList.Count;
                return itemList[this.currentIndex];
            }
        }
    }
}
