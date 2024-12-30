using System.Collections.Generic;
using Attrition.Common.Picking.Strategies;
using NUnit.Framework;

namespace Attrition.Runtime.Common.Picking.Tests
{
    [TestFixture]
    public class RandomPickStrategyTests : PickStrategyTests
    {
        protected override IPickStrategy<int> CreateStrategy()
        {
            return new RandomPickStrategy<int>();
        }
    }
}
