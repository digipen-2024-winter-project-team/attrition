using System;
using System.Collections.Generic;
using Attrition.Common.Picking.Decorators;

namespace Attrition.Common.Picking.Builder
{
    /// <summary>
    /// Provides methods to further configure a picker with additional strategies or constraints.
    /// </summary>
    /// <typeparam name="T">The type of items to pick from.</typeparam>
    public class PickerWithStrategyBuilder<T>
    {
        private Picker<T> picker;

        /// <summary>
        /// Initializes a new instance of the <see cref="PickerWithStrategyBuilder{T}"/> class.
        /// </summary>
        /// <param name="picker">The base picker to configure.</param>
        public PickerWithStrategyBuilder(Picker<T> picker)
        {
            this.picker = picker;
        }

        /// <summary>
        /// Configures the picker to ensure unique selection from the given collection.
        /// </summary>
        /// <param name="fromItems">The collection of items to ensure unique selection from.</param>
        /// <returns>The builder configured with a unique pick strategy decorator.</returns>
        public PickerWithStrategyBuilder<T> UniqueFrom(IEnumerable<T> fromItems)
        {
            var decorator = new UniquePickStrategyDecorator<T>(this.picker.Strategy, fromItems);
            return this.WithDecorator(decorator);
        }
        
        /// <summary>
        /// Configures the picker to allow picking only from a whitelist of items.
        /// </summary>
        /// <param name="whitelist">The collection of whitelisted items.</param>
        /// <returns>The builder configured with a whitelist pick strategy decorator.</returns>
        public PickerWithStrategyBuilder<T> IncludeOnlyIn(IEnumerable<T> whitelist)
        {
            var decorator = new WhitelistPickStrategyDecorator<T>(this.picker.Strategy, whitelist);
            return this.WithDecorator(decorator);
        }

        /// <summary>
        /// Configures the picker to prevent picking from a blacklist of items.
        /// </summary>
        /// <param name="blacklist">The collection of blacklisted items.</param>
        /// <returns>The builder configured with a blacklist pick strategy decorator.</returns>
        public PickerWithStrategyBuilder<T> ExcludeAnyIn(IEnumerable<T> blacklist)
        {
            var decorator = new BlacklistPickStrategyDecorator<T>(this.picker.Strategy, blacklist);
            return this.WithDecorator(decorator);
        }
        
        /// <summary>
        /// Adds a decorator to the existing strategy within the picker.
        /// </summary>
        /// <param name="decorator">The decorator to wrap the current strategy.</param>
        /// <returns>The current builder instance with the decorator applied.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided decorator is null.</exception>
        public PickerWithStrategyBuilder<T> WithDecorator(PickStrategyDecorator<T> decorator)
        {
            if (decorator == null)
            {
                throw new ArgumentNullException(nameof(decorator), "Decorator must not be null.");
            }

            this.picker = new(decorator);
            return this;
        }
        
        /// <summary>
        /// Builds the picker with the configured strategies.
        /// </summary>
        /// <returns>The fully configured picker.</returns>
        public Picker<T> Build()
        {
            return this.picker;
        }
    }
}
