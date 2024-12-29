using System.Linq;
using Attrition.Common.Graphing;
using NUnit.Framework;

namespace Attrition.Runtime.Common.Graphing.Tests.EditMode
{
    [TestFixture]
    public class GraphTests
    {
        private IGraph<string, int> graph;

        [SetUp]
        public void SetUp()
        {
            this.graph = new MockGraph<string, int>();
        }
        
        [Test]
        public void GivenGraphWithNodes_WhenNodesRequested_ThenReturnsAllNodes()
        {
            /* ARRANGE */
            var nodeA = this.graph.AddNode("A");
            var nodeB = this.graph.AddNode("B");

            /* ACT */
            var nodes = this.graph.Nodes.ToList();

            /* ASSERT */
            Assert.NotNull(nodes);
            Assert.IsTrue(nodes.Contains(nodeA));
            Assert.IsTrue(nodes.Contains(nodeB));
        }
        
        [Test]
        public void GivenGraphWithEdges_WhenEdgesRequested_ThenReturnsAllEdges()
        {
            /* ARRANGE */
            var nodeA = this.graph.AddNode("A");
            var nodeB = this.graph.AddNode("B");
            var edge = this.graph.AddEdge(nodeA, nodeB, 1);

            /* ACT */
            var edges = this.graph.Edges;

            /* ASSERT */
            Assert.NotNull(edges);
            Assert.IsTrue(edges.Contains(edge));
        }

        [Test]
        public void GivenEmptyUndirectedGraph_WhenNodeAddedByValue_ThenGraphContainsNode()
        {
            /* ARRANGE */
            var nodeValue = "TestNode";

            /* ACT */
            var node = this.graph.AddNode(nodeValue);

            /* ASSERT */
            Assert.NotNull(node);
            Assert.NotNull(this.graph.Nodes);
            Assert.IsTrue(this.graph.Nodes.Contains(node));
        }

        [Test]
        public void GivenEmptyUndirectedGraph_WhenNodeAddedByNode_ThenGraphContainsNode()
        {
            /* ARRANGE */
            var node = new Node<string, int>(this.graph, "TestNode");

            /* ACT */
            this.graph.AddNode(node);

            /* ASSERT */
            Assert.NotNull(this.graph.Nodes);
            Assert.IsTrue(this.graph.Nodes.Contains(node));
        }
        
        [Test]
        public void GivenGraphWithNodes_WhenNodeRemoved_ThenNodeNotInGraph()
        {
            /* ARRANGE */
            var node = this.graph.AddNode("TestNode");

            /* ACT */
            this.graph.RemoveNode(node);

            /* ASSERT */
            Assert.NotNull(this.graph.Nodes);
            Assert.IsFalse(this.graph.Nodes.Contains(node));
        }
        
        [Test]
        public void GivenGraphWithNodes_WhenEdgeAdded_ThenEdgeInGraph()
        {
            /* ARRANGE */
            var nodeA = this.graph.AddNode("A");
            var nodeB = this.graph.AddNode("B");

            /* ACT */
            var edge = this.graph.AddEdge(nodeA, nodeB, 1);

            /* ASSERT */
            Assert.NotNull(edge);
            Assert.NotNull(this.graph.Edges);
            Assert.IsTrue(this.graph.Edges.Contains(edge));
        }
        
        [Test]
        public void GivenGraphWithEdges_WhenEdgeRemoved_ThenEdgeNotInGraph()
        {
            /* ARRANGE */
            var nodeA = this.graph.AddNode("A");
            var nodeB = this.graph.AddNode("B");
            var edge = this.graph.AddEdge(nodeA, nodeB, 1);

            /* ACT */
            this.graph.RemoveEdge(edge);

            /* ASSERT */
            Assert.NotNull(this.graph.Edges);
            Assert.IsFalse(this.graph.Edges.Contains(edge));
        }
        
        [Test]
        public void GivenGraphWithNodes_WhenGraphCleared_ThenGraphIsEmpty()
        {
            /* ARRANGE */
            var nodeA = this.graph.AddNode("A");
            var nodeB = this.graph.AddNode("B");
            var edge = this.graph.AddEdge(nodeA, nodeB, 1);

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
