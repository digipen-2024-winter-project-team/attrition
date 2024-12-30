using System;
using Attrition.Common.Picking;
using Attrition.Common.Picking.Builder;
using Attrition.Common.Picking.Strategies;
using NUnit.Framework;

namespace Attrition.Runtime.Common.Picking.Tests
{
    [TestFixture]
    public class PickerBuilderTests
    {
        /// <summary>
        /// Tests that WithDecorator applies a mock decorator to the current picker.
        /// </summary>
        [Test]
        public void GivenPickerWithStrategyBuilder_WhenWithDecoratorCalled_ThenDecoratorIsApplied()
        {
            /* ARRANGE */
            var initialStrategy = new RoundRobinPickStrategy<int>();
            var picker = new Picker<int>(initialStrategy);
            var builder = new PickerWithStrategyBuilder<int>(picker);
            var decorator = new MockPickStrategyDecorator<int>(initialStrategy);

            /* ACT */
            builder.WithDecorator(decorator);
            var resultPicker = builder.Build();

            /* ASSERT */
            Assert.IsInstanceOf<MockPickStrategyDecorator<int>>(resultPicker.Strategy);
        }

        /// <summary>
        /// Tests that WithDecorator throws an exception when called with a null decorator.
        /// </summary>
        [Test]
        public void GivenPickerWithStrategyBuilder_WhenWithDecoratorCalledWithNull_ThenThrowsArgumentNullException()
        {
            /* ARRANGE */
            var picker = new Picker<int>(new RoundRobinPickStrategy<int>());
            var builder = new PickerWithStrategyBuilder<int>(picker);

            /* ACT */
            void Action() => builder.WithDecorator(null);

            /* ASSERT */
            Assert.Throws<ArgumentNullException>(Action);
        }

        /// <summary>
        /// Tests that UseStrategy initializes a Picker with the provided strategy.
        /// </summary>
        [Test]
        public void GivenUseStrategy_WhenCalledWithValidStrategy_ThenPickerIsConfiguredCorrectly()
        {
            /* ARRANGE */
            var strategy = new MockPickStrategy<int>(1);
            var builder = new PickerBuilder<int>();

            /* ACT */
            var pickerBuilder = builder.UseStrategy(strategy);
            var picker = pickerBuilder.Build();

            /* ASSERT */
            Assert.AreEqual(strategy, picker.Strategy);
        }

        /// <summary>
        /// Tests that UseStrategy throws an exception when a null strategy is provided.
        /// </summary>
        [Test]
        public void GivenUseStrategy_WhenCalledWithNullStrategy_ThenThrowsArgumentNullException()
        {
            /* ARRANGE */
            var builder = new PickerBuilder<int>();

            /* ACT */
            void Action() => builder.UseStrategy(null);

            /* ASSERT */
            Assert.Throws<System.ArgumentNullException>(Action);
        }

        /// <summary>
        /// Tests that PickerBuilder chaining works as expected.
        /// </summary>
        [Test]
        public void GivenPickerBuilder_WhenChained_ThenBuildsPickerCorrectly()
        {
            /* ARRANGE */
            var builder = new PickerBuilder<int>();
            var strategy = new MockPickStrategy<int>(1);

            /* ACT */
            var picker = builder.UseStrategy(strategy).Build();

            /* ASSERT */
            Assert.IsNotNull(picker);
            Assert.AreEqual(strategy, picker.Strategy);
        }

        /// <summary>
        /// Tests that chaining multiple decorators applies them in the correct order.
        /// </summary>
        [Test]
        public void GivenPickerWithStrategyBuilder_WhenChainedWithMultipleDecorators_ThenAppliesDecoratorsInOrder()
        {
            /* ARRANGE */
            var initialStrategy = new RoundRobinPickStrategy<int>();
            var picker = new Picker<int>(initialStrategy);
            var builder = new PickerWithStrategyBuilder<int>(picker);

            var decorator1 = new MockPickStrategyDecorator<int>(initialStrategy);
            var decorator2 = new MockPickStrategyDecorator<int>(decorator1);

            /* ACT */
            builder.WithDecorator(decorator1).WithDecorator(decorator2);
            var resultPicker = builder.Build();

            /* ASSERT */
            Assert.IsInstanceOf<MockPickStrategyDecorator<int>>(resultPicker.Strategy);
            Assert.AreEqual(decorator2, resultPicker.Strategy);
        }
    }
}
