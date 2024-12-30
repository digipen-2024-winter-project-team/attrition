using System.Collections.Generic;
using Attrition.Common.Picking.Strategies;

namespace Attrition.Runtime.Common.Picking.Tests
{
    /// <summary>
    /// A mock implementation of IPickStrategy for testing purposes.
    /// </summary>
    public class MockPickStrategy<T> : IPickStrategy<T>
    {
        public bool WasPickCalled { get; private set; }
        private readonly T returnValue;

        public MockPickStrategy(T returnValue)
        {
            this.returnValue = returnValue;
        }

        public T Pick(IEnumerable<T> items)
        {
            this.WasPickCalled = true;
            return this.returnValue;
        }
    }
}
