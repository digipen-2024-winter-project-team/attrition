﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using Attrition.Common.Picking.Strategies;
using Attrition.Common.Picking.Decorators;

namespace Attrition.Runtime.Common.Picking.Tests
{
    /// <summary>
    /// Base class for testing implementations of <see cref="PickStrategyDecorator{T}"/>.
    /// Provides common test cases to validate the behavior of decorators.
    /// </summary>
    /// <typeparam name="T">The type of items being picked.</typeparam>
    public abstract class PickerStrategyDecoratorTests<T>
    {
        /// <summary>
        /// Creates an instance of the decorator to be tested.
        /// This method is implemented by derived test classes to provide the specific decorator under test.
        /// </summary>
        /// <param name="innerStrategy">The underlying strategy to be decorated.</param>
        /// <returns>A new instance of the <see cref="PickStrategyDecorator{T}"/>.</returns>
        protected abstract PickStrategyDecorator<T> CreateDecorator(IPickStrategy<T> innerStrategy);

        /// <summary>
        /// Tests that the decorator forwards the <c>Pick</c> method call to the underlying strategy.
        /// </summary>
        [Test]
        public void GivenDecorator_WhenPickCalled_ThenUnderlyingStrategyIsInvoked()
        {
            /* ARRANGE */
            var mockStrategy = new MockPickStrategy<T>(default);
            var decorator = this.CreateDecorator(mockStrategy);
            var items = new List<T> { default };

            /* ACT */
            decorator.Pick(items);

            /* ASSERT */
            Assert.IsTrue(mockStrategy.WasPickCalled);
        }

        /// <summary>
        /// Tests that an exception is thrown when <c>Pick</c> is called with a null collection.
        /// </summary>
        [Test]
        public void GivenNullCollection_WhenPickCalled_ThenThrowsArgumentException()
        {
            /* ARRANGE */
            var mockStrategy = new MockPickStrategy<T>(default);
            var decorator = this.CreateDecorator(mockStrategy);

            /* ACT */
            void Action() => decorator.Pick(null);

            /* ASSERT */
            Assert.Throws<ArgumentException>(Action);
        }

        /// <summary>
        /// Tests that an exception is thrown when <c>Pick</c> is called with an empty collection.
        /// </summary>
        [Test]
        public void GivenEmptyCollection_WhenPickCalled_ThenThrowsArgumentException()
        {
            /* ARRANGE */
            var mockStrategy = new MockPickStrategy<T>(default);
            var decorator = this.CreateDecorator(mockStrategy);
            var items = new List<T>();

            /* ACT */
            void Action() => decorator.Pick(items);

            /* ASSERT */
            Assert.Throws<ArgumentException>(Action);
        }
    }
}
