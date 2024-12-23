using System.Collections.Generic;
using System.Linq;
using Attrition.Common.Picking;
using NUnit.Framework;

namespace Attrition.Runtime.Common.Picking.Tests
{
    /// <summary>
    /// Tests for the Picker class.
    /// </summary>
    [TestFixture]
    public class PickerTests
    {
        /// <summary>
        /// Tests that the Picker delegates picking to the configured strategy.
        /// </summary>
        [Test]
        public void GivenConfiguredStrategy_WhenPickInvoked_ThenStrategyPickIsCalled()
        {
            /* ARRANGE */
            var mockStrategy = new MockPickStrategy<int>(1);
            var picker = new Picker<int>(mockStrategy);
            var items = new List<int> { 1, 2, 3 };

            /* ACT */
            var result = picker.Pick(items);

            /* ASSERT */
            Assert.AreEqual(1, result);
        }

        /// <summary>
        /// Tests that Picker can handle multiple picks.
        /// </summary>
        [Test]
        public void GivenConfiguredStrategy_WhenPickInvokedMultipleTimes_ThenDelegatesEachTime()
        {
            /* ARRANGE */
            var strategy = new MockPickStrategy<int>(1);
            var picker = new Picker<int>(strategy);
            var items = new List<int> { 1, 2, 3 };

            /* ACT */
            var result1 = picker.Pick(items);
            var result2 = picker.Pick(items);
            var result3 = picker.Pick(items);

            /* ASSERT */
            Assert.AreEqual(1, result1);
            Assert.AreEqual(1, result2);
            Assert.AreEqual(1, result3);
        }
    }
}
