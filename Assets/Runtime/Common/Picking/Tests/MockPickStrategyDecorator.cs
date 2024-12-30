using Attrition.Common.Picking.Decorators;
using Attrition.Common.Picking.Strategies;

namespace Attrition.Runtime.Common.Picking.Tests
{
    /// <summary>
    /// A mock implementation of <see cref="PickStrategyDecorator{T}"/> for testing purposes.
    /// Used to validate behavior when decorating pick strategies in tests.
    /// </summary>
    /// <typeparam name="T">The type of items being picked.</typeparam>
    public class MockPickStrategyDecorator<T> : PickStrategyDecorator<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockPickStrategyDecorator{T}"/> class.
        /// </summary>
        /// <param name="innerStrategy">The inner pick strategy to be decorated.</param>
        public MockPickStrategyDecorator(IPickStrategy<T> innerStrategy) : base(innerStrategy) { }
    }
}
