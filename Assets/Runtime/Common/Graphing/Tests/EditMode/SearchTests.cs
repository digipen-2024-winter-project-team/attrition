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

        [SetUp]
        public void SetUp()
        {
            this.graph = new();
            this.searcher = new(this.graph);
        }
        
        
    }
}
