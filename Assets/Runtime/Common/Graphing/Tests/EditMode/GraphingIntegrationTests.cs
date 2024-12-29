using System.Collections;
using Attrition.Common.Graphing;
using NUnit.Framework;

namespace Attrition.Runtime.Common.Graphing.Tests.EditMode
{
    [TestFixture]
    [Category("Integration")]
    public class GraphingIntegrationTests
    {
        private IGraph<string, int> graph;
        
        private Node<string, int> foo;
        private Node<string, int> bar;
        private Node<string, int> baz;
        private Node<string, int> qux;
        private Node<string, int> quux;

        [SetUp]
        public void SetUp()
        {
            this.graph = new Graph<string, int>();
            
            this.foo = new(this.graph, "foo");
            this.bar = new(this.graph, "bar");
            this.baz = new(this.graph, "baz");
            this.qux = new(this.graph, "qux");
            this.quux = new(this.graph, "quux");
        }

        // [Test]
        // public void GivenEmptyGraph_WhenNodeAdded_ThenGraphContainsNode()
        // {
        //     /* ARRANGE */
        //     
        //     
        //     /* ACT */
        //     var first = this.graph.AddNode(this.foo);
        //     
        //     /* ASSERT */
        //     Assert.NotNull(first);
        //     Assert.AreSame(this.foo, first);
        //     Assert.Contains(this.foo, (ICollection)this.graph.Nodes);
        // }
    }
}
