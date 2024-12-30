using System.Linq;
using Attrition.Common.Graphing;
using NUnit.Framework;

namespace Attrition.Runtime.Common.Graphing.Tests.EditMode
{
    [TestFixture]
    [Category(TestCategories.Integration)]
    public class SearchTests
    {
        private MockGraph<string, int> graph;
        private BreadthFirstGraphSearcher<string, int> searcher;
        private INode<string, int> foo;
        private INode<string, int> bar;
        private INode<string, int> baz;
        private INode<string, int> qux;
        private INode<string, int> quux;
        private IEdge<string, int> foobar;
        private IEdge<string, int> barbaz;
        private IEdge<string, int> bazqux;
        private IEdge<string, int> quxquux;
        private IEdge<string, int> quuxfoo;
        private IEdge<string, int> barqux;
        private IEdge<string, int> quxfoo;
        private IEdge<string, int> bazquux;

        [SetUp]
        public void SetUp()
        {
            this.graph = new();
            this.searcher = new(this.graph);

            this.foo = new Node<string, int>(this.graph, "foo");
            this.bar = new Node<string, int>(this.graph, "bar");
            this.baz = new Node<string, int>(this.graph, "baz");
            this.qux = new Node<string, int>(this.graph, "qux");
            this.quux = new Node<string, int>(this.graph, "quux");
            
            this.foobar = new Edge<string, int>(this.graph, this.foo, this.bar, 1);
            this.barbaz = new Edge<string, int>(this.graph, this.bar, this.baz, 1);
            this.bazqux = new Edge<string, int>(this.graph, this.baz, this.qux, 1);
            this.quxquux = new Edge<string, int>(this.graph, this.qux, this.quux, 1);
            this.quuxfoo = new Edge<string, int>(this.graph, this.quux, this.foo, 1);
            this.barqux = new Edge<string, int>(this.graph, this.bar, this.qux, 1);
            this.quxfoo = new Edge<string, int>(this.graph, this.qux, this.foo, 1);
            this.bazquux = new Edge<string, int>(this.graph, this.baz, this.quux, 1);
        }

        [Test]
        public void GivenEmptyGraph_WhenSearchCalled_ThenThrowsGraphSearchException()
        {
            /* ARRANGE */
            

            /* ACT */
            void Action()
            {
                var results = this.searcher.Search(this.foo, this.quux);
            }
            
            /* ASSERT */
            Assert.Throws<GraphSearchException<string, int>>(Action);
        }

        [Test]
        public void GivenGraphWithNoPath_WhenSearchCalled_ThenReturnsVisitedNodes()
        {
            /* ARRANGE */
            this.PopulateGraphWithSimplePath();

            /* ACT */
            var result = this.searcher.Search(this.foo, this.quux).ToList();

            /* ASSERT */
            Assert.Contains(this.foo, result);
            Assert.Contains(this.quux, result);
            Assert.IsFalse(result.Contains(this.qux));
        }

        [Test]
        public void GivenGraphWithPath_WhenSearchCalled_ThenReturnsPathToTarget()
        {
            /* ARRANGE */
            this.PopulateGraphWithSimplePath();

            /* ACT */
            var result = this.searcher.Search(this.foo, this.quux).ToList();

            /* ASSERT */
            Assert.AreEqual(new[] { this.foo, this.bar, this.baz, this.quux }, result);
        }

        [Test]
        public void GivenGraphWithCycles_WhenSearchCalled_ThenHandlesCyclesCorrectly()
        {
            /* ARRANGE */
            this.PopulateGraphWithCyclicalPath();

            /* ACT */
            var result = this.searcher.Search(this.foo, this.quux).ToList();

            /* ASSERT */
            Assert.AreEqual(new[] { this.foo, this.bar, this.baz, this.qux, this.quux }, result);
        }

        [Test]
        public void GivenGraph_WhenStartNodeEqualsTargetNode_ThenReturnsSingleNode()
        {
            /* ARRANGE */
            this.PopulateGraphWithSimplePath();

            /* ACT */
            var result = this.searcher.Search(this.foo, this.foo).ToList();

            /* ASSERT */
            Assert.AreEqual(new[] { this.foo }, result);
        }
        
        private void PopulateGraphWithSimplePath()
        {
            this.graph.AddNode(this.foo);
            this.graph.AddNode(this.bar);
            this.graph.AddNode(this.baz);
            this.graph.AddNode(this.qux);
            this.graph.AddNode(this.quux);
            
            this.graph.AddEdge(this.foobar);
            this.graph.AddEdge(this.barbaz);
            this.graph.AddEdge(this.bazquux);
            this.graph.AddEdge(this.quxquux);
        }

        private void PopulateGraphWithCyclicalPath()
        {
            this.graph.AddNode(this.foo);
            this.graph.AddNode(this.bar);
            this.graph.AddNode(this.baz);
            this.graph.AddNode(this.qux);
            this.graph.AddNode(this.quux);
            
            this.graph.AddEdge(this.foobar);
            this.graph.AddEdge(this.barbaz);
            this.graph.AddEdge(this.bazqux);
            this.graph.AddEdge(this.quxquux);
            this.graph.AddEdge(this.quuxfoo);
        }
    }
}
