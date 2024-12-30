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
        }

        [Test]
        public void GivenEmptyGraph_WhenSearchCalled_ThenReturnsEmpty()
        {
            /* ARRANGE */
            

            /* ACT */
            var result = this.searcher.Search(this.foo, this.quux);

            /* ASSERT */
            Assert.IsEmpty(result);
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
