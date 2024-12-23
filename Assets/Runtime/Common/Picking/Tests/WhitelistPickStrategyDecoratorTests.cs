using System.Collections.Generic;
using Attrition.Common.Picking.Builder;
using Attrition.Common.Picking.Decorators;
using Attrition.Common.Picking.Strategies;
using NUnit.Framework;

namespace Attrition.Runtime.Common.Picking.Tests
{
    public class WhitelistPickStrategyDecoratorTests : PickerStrategyDecoratorTests
    {
        private PickerBuilder<int> pickerBuilder;

        [SetUp]
        public void SetUp()
        {
            this.pickerBuilder = new();
        }

        /// <summary>
        /// Ensures that a picker configured with the `IncludeOnlyIn` decorator only picks from the whitelist.
        /// </summary>
        [Test]
        public void GivenWhitelist_WhenPicking_ThenOnlyWhitelistItemsArePicked()
        {
            // ARRANGE
            var availableItems = new List<int> { 1, 2, 3, 4 };
            var whitelist = new List<int> { 2, 3 };
            var picker = this.pickerBuilder
                .UseRoundRobin()
                .IncludeOnlyIn(whitelist)
                .Build();

            // ACT
            var pickedItem = picker.Pick(availableItems);

            // ASSERT
            Assert.IsTrue(whitelist.Contains(pickedItem), "Picker should only pick items from the whitelist.");
        }

        protected override PickStrategyDecorator<T> CreateDecorator<T>(IPickStrategy<T> innerStrategy, IEnumerable<T> contents)
        {
            return new WhitelistPickStrategyDecorator<T>(innerStrategy, contents);
        }
    }
}
