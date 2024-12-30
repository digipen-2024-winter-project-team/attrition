using System.Collections.Generic;

namespace Attrition.Common.Picking.Strategies
{
    /// <summary>
    /// Defines the contract for a strategy to pick an item from a collection.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public interface IPickStrategy<T>
    {
        /// <summary>
        /// Picks an item from the given collection.
        /// </summary>
        /// <param name="items">The collection of items to pick from.</param>
        /// <returns>The selected item.</returns>
        T Pick(IEnumerable<T> items);
    }
}
