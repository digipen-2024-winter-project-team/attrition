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
        private IEdge<string, int> foobaz;
        private IEdge<string, int> fooqux;
        private IEdge<string, int> fooquux;
        private IEdge<string, int> barbaz;
        private IEdge<string, int> bazqux;
        private IEdge<string, int> quxquux;

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
            this.foobaz = new Edge<string, int>(this.graph, this.foo, this.baz, 1);
            this.fooqux = new Edge<string, int>(this.graph, this.foo, this.qux, 1);
            this.fooquux = new Edge<string, int>(this.graph, this.foo, this.quux, 1);
            this.barbaz = new Edge<string, int>(this.graph, this.bar, this.baz, 1);
            this.bazqux = new Edge<string, int>(this.graph, this.baz, this.qux, 1);
            this.quxquux = new Edge<string, int>(this.graph, this.qux, this.quux, 1);
        }

        [Test]
        public void GivenAGraphWithNeighboringNodes_WhenSearched_ItShouldReturnNeighborNodesFirst()
        {
            /* ARRANGE */
            this.graph.AddNode(this.foo);
            this.graph.AddNode(this.bar);
            this.graph.AddNode(this.baz);
            this.graph.AddNode(this.qux);
            this.graph.AddNode(this.quux);
            
            this.graph.AddEdge(this.foobar);
            this.graph.AddEdge(this.foobaz);
            this.graph.AddEdge(this.fooqux);
            
            this.graph.AddEdge(this.barbaz);
            this.graph.AddEdge(this.bazqux);
            this.graph.AddEdge(this.quxquux);
            
            /* ACT */
            var result = this.searcher.Search(this.foo, this.quux);

            /* ASSERT */
            var expected = new[]
            {
                this.foo,
                this.bar,
                this.baz,
                this.qux,
                this.quux,
            };
            
            Assert.That(result, Is.EquivalentTo(expected));
        }
    }
}
