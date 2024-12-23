using System.Collections.Generic;
using Attrition.Common.Picking.Builder;
using Attrition.Common.Picking.Decorators;
using Attrition.Common.Picking.Strategies;
using NUnit.Framework;

namespace Attrition.Runtime.Common.Picking.Tests
{
    public class BlacklistPickStrategyDecoratorTests : PickerStrategyDecoratorTests
    {
        private PickerBuilder<int> pickerBuilder;
        
        [SetUp]
        public void SetUp()
        {
            this.pickerBuilder = new();
        }
        
        /// <summary>
        /// Ensures that a picker configured with the `Prevent` decorator does not pick blacklisted items.
        /// </summary>
        [Test]
        public void GivenBlacklist_WhenPicking_ThenBlacklistedItemsAreNotPicked()
        {
            // ARRANGE
            var availableItems = new List<int> { 1, 2, 3, 4 };
            var blacklist = new List<int> { 2, 4 };
            var picker = pickerBuilder
                .UseRandom()
                .ExcludeAnyIn(blacklist)
                .Build();

            // ACT
            var pickedItem = picker.PickFrom(availableItems);

            // ASSERT
            Assert.IsFalse(blacklist.Contains(pickedItem), "Picker should not pick items from the blacklist.");
        }

        protected override PickStrategyDecorator<T> CreateDecorator<T>(IPickStrategy<T> innerStrategy, IEnumerable<T> contents)
        {
            return new BlacklistPickStrategyDecorator<T>(innerStrategy, contents);
        }
    }
}
