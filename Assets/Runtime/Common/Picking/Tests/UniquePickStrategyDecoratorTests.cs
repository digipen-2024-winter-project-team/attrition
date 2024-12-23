using Attrition.Common.Picking.Decorators;
using Attrition.Common.Picking.Strategies;
using NUnit.Framework;

namespace Attrition.Runtime.Common.Picking.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="UniquePickStrategyDecorator{T}"/> class.
    /// Verifies that the decorator ensures unique items are picked 
    /// and prevents consecutive duplicates.
    /// </summary>
    [TestFixture]
    public class UniquePickStrategyDecoratorTests : PickerStrategyDecoratorTests<int>
    {
        /// <summary>
        /// Creates an instance of <see cref="UniquePickStrategyDecorator{T}"/> 
        /// with the specified inner strategy for testing purposes.
        /// </summary>
        /// <param name="innerStrategy">The inner strategy to be decorated.</param>
        /// <returns>A new instance of <see cref="UniquePickStrategyDecorator{T}"/>.</returns>
        protected override PickStrategyDecorator<int> CreateDecorator(IPickStrategy<int> innerStrategy)
        {
            return new UniquePickStrategyDecorator<int>(innerStrategy);
        }
    }
}
