using System.Linq;
using UnityEngine;

namespace Attrition.Common.Graphing
{
    public class GraphingSample : MonoBehaviour
    {
        private IGraph<string, float> graph;
        private Node<string, float> foo;
        private Node<string, float> bar;
        private Node<string, float> baz;
        private Edge<string, float> foobar;
        private Edge<string, float> barbaz;

        public IGraph<string, float> BuildGraph()
        {
            this.foo = new(this.graph, "foo");
            this.bar = new(this.graph, "bar");
            this.baz = new(this.graph, "baz");
            this.foobar = new(this.graph, this.foo, this.bar);
            this.barbaz = new(this.graph, this.bar, this.baz);
            
            this.graph.AddNode(this.foo);
            this.graph.AddNode(this.bar);
            this.graph.AddNode(this.baz);
            this.graph.AddEdge(this.foobar);
            this.graph.AddEdge(this.barbaz);
            
            return this.graph;
        }

        public void TraverseGraph()
        {
            var enumerator = new BreadthFirstGraphEnumerator();
            
            foreach (var node in enumerator.Enumerate(this.graph, this.foo))
            {
                Debug.Log($"{node.Value} :: Has {node.GetNeighbors().Count()} neighbors.");
            }
        }

        private void Awake()
        {
            this.graph = new UndirectedGraph<string, float>();
        }

        private void OnEnable()
        {
            this.graph = this.BuildGraph();
            this.TraverseGraph();
        }
    }
}
