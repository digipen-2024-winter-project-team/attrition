using System.Collections.Generic;
using Attrition.Common.Picking.Strategies;
using NUnit.Framework;

namespace Attrition.Runtime.Common.Picking.Tests
{
    public abstract class PickStrategyTests
    {
        protected abstract IPickStrategy<int> CreateStrategy();

        /// <summary>
        /// Tests that an exception is raised when Pick is invoked on a null collection.
        /// </summary>
        [Test]
        public void GivenNullCollection_WhenPickInvoked_ThenExceptionShouldBeRaised()
        {
            /* ARRANGE */
            var strategy = this.CreateStrategy();

            /* ACT */
            void Action() => strategy.Pick(null);

            /* ASSERT */
            Assert.Throws<System.ArgumentException>(Action);
        }

        /// <summary>
        /// Tests that an exception is raised when Pick is invoked on an empty collection.
        /// </summary>
        [Test]
        public void GivenEmptyCollection_WhenPickInvoked_ThenExceptionShouldBeRaised()
        {
            /* ARRANGE */
            var strategy = this.CreateStrategy();

            /* ACT */
            void Action() => strategy.Pick(new List<int>());

            /* ASSERT */
            Assert.Throws<System.ArgumentException>(Action);
        }
        
        /// <summary>
        /// Tests that an item from the collection is returned when pick is invoked on a non-empty collection.
        /// </summary>
        [Test]
        public void GivenNonEmptyCollection_WhenRandomPickInvoked_ThenItemShouldBeFromCollection()
        {
            /* ARRANGE */
            var strategy = this.CreateStrategy();
            var items = new List<int> { 1, 2, 3, 4 };

            /* ACT */
            var result = strategy.Pick(items);

            /* ASSERT */
            Assert.Contains(result, items);
        }
    }
}
