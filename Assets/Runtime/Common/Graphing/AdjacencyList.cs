using System.Collections.Generic;
using System.Linq;

namespace Attrition.Common.Graphing
{
    public class AdjacencyList<TNodeData, TEdgeData> : IAdjacencyList<TNodeData, TEdgeData>
    {
        private readonly Dictionary<INode<TNodeData, TEdgeData>, List<IEdge<TNodeData, TEdgeData>>> dictionary;

        public IEnumerable<INode<TNodeData, TEdgeData>> Nodes => this.dictionary.Keys;
        public IEnumerable<IEdge<TNodeData, TEdgeData>> Edges => this.dictionary.Values
            .SelectMany(edges => edges);
        
        public AdjacencyList()
        {
            this.dictionary = new();
        }

        public void AddNode(INode<TNodeData, TEdgeData> node)
        {
            if (!this.dictionary.ContainsKey(node))
            {
                this.dictionary[node] = new();
            }
        }

        public void RemoveNode(INode<TNodeData, TEdgeData> node)
        {
            if (this.dictionary.Remove(node))
            {
                foreach (var edges in this.dictionary.Values)
                {
                    edges.RemoveAll(edge => edge.To.Equals(node));
                }
            }
        }

        public void AddEdge(IEdge<TNodeData, TEdgeData> edge)
        {
            var node = edge.From;
            
            if (!this.dictionary.ContainsKey(node))
            {
                this.AddNode(node);
            }

            this.dictionary[node].Add(edge);
        }

        public void RemoveEdge(IEdge<TNodeData, TEdgeData> edge)
        {
            var node = edge.From;
            
            if (this.dictionary.ContainsKey(node))
            {
                this.dictionary[node].Remove(edge);
            }
        }

        public void RemoveAllEdgesFrom(INode<TNodeData, TEdgeData> node)
        {
            if (this.dictionary.ContainsKey(node))
            {
                this.dictionary[node].Clear();
            }
        }
        
        public IEnumerable<IEdge<TNodeData, TEdgeData>> GetEdgesFrom(INode<TNodeData, TEdgeData> node)
        {
            return this.dictionary.ContainsKey(node) ? this.dictionary[node] : Enumerable.Empty<IEdge<TNodeData, TEdgeData>>();
        }

        public void Clear()
        {
            this.dictionary.Clear();
        }
    }
}
