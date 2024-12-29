using System.Linq;
using Attrition.Common.Graphing;
using NUnit.Framework;

namespace Attrition.Runtime.Common.Graphing.Tests.EditMode
{
    [TestFixture]
    public class GraphTests
    {
        private IGraph<string, int> graph;
        private INode<string, int> foo;
        private INode<string, int> bar;
        private INode<string, int> baz;
        private IEdge<string, int> foobar;
        private IEdge<string, int> barbaz;

        [SetUp]
        public void SetUp()
        {
            this.graph = new MockGraph<string, int>();

            this.foo = new Node<string, int>(this.graph, "foo");
            this.bar = new Node<string, int>(this.graph, "bar");
            this.baz = new Node<string, int>(this.graph, "baz");
            this.foobar = new Edge<string, int>(this.graph, this.foo, this.bar, 1);
            this.barbaz = new Edge<string, int>(this.graph, this.bar, this.baz, 1);
        }
        
        [Test]
        public void GivenGraphWithNodes_WhenNodesRequested_ThenReturnsAllNodes()
        {
            /* ARRANGE */
            this.graph.AddNode(this.foo);
            this.graph.AddNode(this.bar);
            this.graph.AddNode(this.baz);
            
            /* ACT */
            var nodes = this.graph.Nodes.ToList();

            /* ASSERT */
            Assert.NotNull(nodes);
            Assert.IsTrue(nodes.Contains(this.foo));
            Assert.IsTrue(nodes.Contains(this.bar));
            Assert.IsTrue(nodes.Contains(this.baz));
        }
        
        [Test]
        public void GivenGraphWithEdges_WhenEdgesRequested_ThenReturnsAllEdges()
        {
            /* ARRANGE */
            this.graph.AddNode(this.foo);
            this.graph.AddNode(this.bar);
            this.graph.AddNode(this.baz);
            this.graph.AddEdge(this.foobar);
            this.graph.AddEdge(this.barbaz);

            /* ACT */
            var edges = this.graph.Edges.ToList();

            /* ASSERT */
            Assert.NotNull(edges);
            Assert.IsTrue(edges.Contains(this.foobar));
            Assert.IsTrue(edges.Contains(this.barbaz));
        }

        [Test]
        public void GivenEmptyUndirectedGraph_WhenNodeAddedByValue_ThenGraphContainsNode()
        {
            /* ARRANGE */
            

            /* ACT */
            var node = this.graph.AddNode(this.foo);

            /* ASSERT */
            Assert.NotNull(node);
            Assert.NotNull(this.graph.Nodes);
            Assert.IsTrue(this.graph.Nodes.Contains(node));
        }

        [Test]
        public void GivenEmptyUndirectedGraph_WhenNodeAddedByNode_ThenGraphContainsNode()
        {
            /* ARRANGE */
            

            /* ACT */
            this.graph.AddNode(this.foo);

            /* ASSERT */
            Assert.NotNull(this.graph.Nodes);
            Assert.IsTrue(this.graph.Nodes.Contains(this.foo));
        }
        
        [Test]
        public void GivenGraphWithNodes_WhenNodeRemoved_ThenNodeNotInGraph()
        {
            /* ARRANGE */
            this.graph.AddNode(this.foo);
            this.graph.AddNode(this.bar);
            this.graph.AddNode(this.baz);

            /* ACT */
            this.graph.RemoveNode(this.bar);

            /* ASSERT */
            Assert.NotNull(this.graph.Nodes);
            Assert.IsFalse(this.graph.Nodes.Contains(this.bar));
        }
        
        [Test]
        public void GivenGraphWithNodes_WhenEdgeAdded_ThenEdgeInGraph()
        {
            /* ARRANGE */
            this.graph.AddNode(this.foo);
            this.graph.AddNode(this.bar);
            this.graph.AddNode(this.baz);

            /* ACT */
            this.graph.AddEdge(this.foobar);

            /* ASSERT */
            Assert.NotNull(this.foobar);
            Assert.NotNull(this.graph.Edges);
            Assert.IsTrue(this.graph.Edges.Contains(this.foobar));
        }
        
        [Test]
        public void GivenGraphWithEdges_WhenEdgeRemoved_ThenEdgeNotInGraph()
        {
            /* ARRANGE */
            this.graph.AddNode(this.foo);
            this.graph.AddNode(this.bar);
            this.graph.AddNode(this.baz);
            this.graph.AddEdge(this.foobar);
            this.graph.AddEdge(this.barbaz);

            /* ACT */
            this.graph.RemoveEdge(this.foobar);

            /* ASSERT */
            Assert.NotNull(this.graph.Edges);
            Assert.IsFalse(this.graph.Edges.Contains(this.foobar));
        }
        
        [Test]
        public void GivenGraphWithNodes_WhenGraphCleared_ThenGraphIsEmpty()
        {
            /* ARRANGE */
            this.graph.AddNode(this.foo);
            this.graph.AddNode(this.bar);
            this.graph.AddNode(this.baz);

            this.graph.AddEdge(this.foobar);
            this.graph.AddEdge(this.barbaz);

            /* ACT */
            this.graph.Clear();

            /* ASSERT */
            Assert.NotNull(this.graph.Nodes);
            Assert.IsEmpty(this.graph.Nodes);
            Assert.NotNull(this.graph.Edges);
            Assert.IsEmpty(this.graph.Edges);
        }
    }
}
