using UnityEngine;

namespace Attrition.Common.Graphing
{
    public class GraphingSample
    {
        private readonly IGraph<string, float> graph;
        private Node<string, float> foo;
        private Node<string, float> bar;
        private Node<string, float> baz;
        private Edge<string, float> foobar;
        private Edge<string, float> barbaz;

        public GraphingSample()
        {
            this.graph = new UndirectedGraph<string, float>();
            this.graph = this.BuildGraph();
        }

        public IGraph<string, float> BuildGraph()
        {
            this.foo = new(graph, "foo");
            this.bar = new(graph, "bar");
            this.baz = new(graph, "baz");
            this.foobar = new(graph, this.foo, this.bar);
            this.barbaz = new(graph, this.bar, this.baz);

            return graph;
        }

        public void TraverseGraph()
        {
            var searcher = new GraphSearcher<string, float>(this.graph);
            
            foreach (var node in searcher.Search(this.graph, this.foo, this.baz))
            {
                Debug.Log(node.Value);
            }
        }
    }
}
