using System;
using System.Collections.Generic;
using System.Linq;
using Attrition.Common.Picking.Strategies;

namespace Attrition.Common.Picking.Decorators
{
    /// <summary>
    /// Decorator that ensures unique selection of items.
    /// </summary>
    /// <typeparam name="T">The type of items to pick from.</typeparam>
    public class UniquePickStrategyDecorator<T> : PickStrategyDecorator<T>
    {
        private readonly HashSet<T> pickedItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniquePickStrategyDecorator{T}"/> class.
        /// </summary>
        /// <param name="innerStrategy">The inner strategy to decorate.</param>
        /// <param name="alreadyPicked">The collection of already picked items, if any.</param>
        public UniquePickStrategyDecorator(IPickStrategy<T> innerStrategy, IEnumerable<T> alreadyPicked = null)
            : base(innerStrategy)
        {
            this.pickedItems = (alreadyPicked != null) ? new(alreadyPicked) : new HashSet<T>();
        }

        /// <summary>
        /// Picks an item from the collection, ensuring it has not been picked before.
        /// </summary>
        /// <param name="items">The collection of items to pick from.</param>
        /// <returns>The selected item.</returns>
        /// <exception cref="ArgumentException">Thrown when the collection is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when all items have already been picked.</exception>
        public override T Pick(IEnumerable<T> items)
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

            lock (this.pickedItems)
            {
                var availableItems = itemList
                    .Except(this.pickedItems)
                    .ToList();
                
                if (availableItems.Count == 0)
                {
                    throw new InvalidOperationException("All items have already been picked.");
                }

                var selectedItem = this.InnerStrategy.Pick(availableItems);
                this.pickedItems.Add(selectedItem);
                return selectedItem;
            }
        }
    }
}
