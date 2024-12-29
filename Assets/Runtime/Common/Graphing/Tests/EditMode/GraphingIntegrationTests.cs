using System.Linq;
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
            this.graph = new UndirectedGraph<string, int>();
            
            this.foo = new(this.graph, "foo");
            this.bar = new(this.graph, "bar");
            this.baz = new(this.graph, "baz");
            this.qux = new(this.graph, "qux");
            this.quux = new(this.graph, "quux");
        }

        [Test]
        public void GivenEmptyUndirectedGraph_WhenNodeAdded_ThenGraphContainsNode()
        {
            /* ARRANGE */
            
            
            /* ACT */
            this.graph.AddNode(this.foo);
            
            /* ASSERT */
            Assert.NotNull(this.graph);
            Assert.NotNull(this.graph.Nodes);
            Assert.IsTrue(this.graph.Nodes.Contains(this.foo));
        }
    }
}
