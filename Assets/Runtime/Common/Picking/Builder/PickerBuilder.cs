using System;
using Attrition.Common.Picking.Strategies;

namespace Attrition.Common.Picking.Builder
{
    /// <summary>
    /// Provides methods to build and configure a picker with various strategies.
    /// </summary>
    /// <typeparam name="T">The type of items to pick from.</typeparam>
    public class PickerBuilder<T>
    {
        /// <summary>
        /// Configures the picker to use a random selection strategy.
        /// </summary>
        /// <returns>A builder configured with a random pick strategy.</returns>
        public PickerWithStrategyBuilder<T> UseRandom()
        {
            var strategy = new RandomPickStrategy<T>();
            return this.UseStrategy(strategy);
        }

        /// <summary>
        /// Configures the picker to use a round-robin selection strategy.
        /// </summary>
        /// <returns>A builder configured with a round-robin pick strategy.</returns>
        public PickerWithStrategyBuilder<T> UseRoundRobin()
        {
            var strategy = new RoundRobinPickStrategy<T>();
            return this.UseStrategy(strategy);
        }
        
        /// <summary>
        /// Configures a Picker instance with the specified pick strategy.
        /// </summary>
        /// <param name="strategy">The pick strategy to be used by the Picker.</param>
        /// <returns>A new instance of PickerWithStrategyBuilder configured with the specified strategy.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided strategy is null.</exception>
        /// <remarks>
        /// This method is a core part of the builder pattern. It allows dynamic configuration of the Picker
        /// by assigning a specific strategy for item selection. Strategies implement the IPickStrategy interface,
        /// enabling flexible and extensible behavior. The method encapsulates the Picker's instantiation, ensuring
        /// the constructed Picker is properly initialized before being returned in a builder for further configuration.
        /// </remarks>
        public PickerWithStrategyBuilder<T> UseStrategy(IPickStrategy<T> strategy)
        {
            if (strategy == null)
            {
                throw new ArgumentNullException(nameof(strategy), "Strategy must not be null.");
            }

            var picker = new Picker<T>(strategy);
            return new(picker);
        }
    }
}
