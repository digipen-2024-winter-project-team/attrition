using System.Collections.Generic;
using Attrition.Common.Picking.Strategies;
using NUnit.Framework;

namespace Attrition.Runtime.Common.Picking.Tests
{
    [TestFixture]
    public class RoundRobinPickStrategyTests : PickStrategyTests
    {
        /// <summary>
        /// Tests that items are picked in round-robin order when the collection is non-empty.
        /// </summary>
        [Test]
        public void GivenNonEmptyCollection_WhenRoundRobinPickInvoked_ThenItemsShouldBePickedInOrder()
        {
            /* ARRANGE */
            var strategy = new RoundRobinPickStrategy<int>();
            var items = new List<int> { 1, 2, 3 };

            /* ACT */
            var result1 = strategy.Pick(items);
            var result2 = strategy.Pick(items);
            var result3 = strategy.Pick(items);
            var result4 = strategy.Pick(items);

            /* ASSERT */
            Assert.AreEqual(1, result1);
            Assert.AreEqual(2, result2);
            Assert.AreEqual(3, result3);
            Assert.AreEqual(1, result4);
        }
        
        protected override IPickStrategy<int> CreateStrategy()
        {
            return new RoundRobinPickStrategy<int>();
        }
    }
}
