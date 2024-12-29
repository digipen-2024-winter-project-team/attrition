using System;
using System.Collections.Generic;
using System.Linq;

namespace Attrition.Common.Graphing
{
    public class AdjacencyList<TNodeData, TEdgeData> : IAdjacencyList<TNodeData, TEdgeData>
    {
        private readonly Dictionary<INode<TNodeData, TEdgeData>, List<IEdge<TNodeData, TEdgeData>>> adjacencies;

        public IEnumerable<INode<TNodeData, TEdgeData>> Nodes => this.adjacencies.Keys;
        public IEnumerable<IEdge<TNodeData, TEdgeData>> Edges => this.adjacencies.Values
            .SelectMany(edges => edges);
        
        public AdjacencyList()
        {
            this.adjacencies = new();
        }

        public void AddNode(INode<TNodeData, TEdgeData> node)
        {
            if (!this.adjacencies.ContainsKey(node))
            {
                this.adjacencies[node] = new();
            }
        }

        public void RemoveNode(INode<TNodeData, TEdgeData> node)
        {
            if (this.adjacencies.Remove(node))
            {
                foreach (var edges in this.adjacencies.Values)
                {
                    edges.RemoveAll(edge => edge.To.Equals(node));
                }
            }
        }

        public void AddEdge(INode<TNodeData, TEdgeData> from, INode<TNodeData, TEdgeData> to, TEdgeData cost = default)
        {
            if (!this.adjacencies.ContainsKey(from))
            {
                throw new InvalidOperationException("The 'from' node is not in the graph.");
            }

            this.adjacencies[from].Add(new Edge<TNodeData, TEdgeData>(null, from, to, cost));
        }

        public IEnumerable<IEdge<TNodeData, TEdgeData>> GetEdgesFrom(INode<TNodeData, TEdgeData> node)
        {
            return this.adjacencies.TryGetValue(node, out var edges) ? edges : Enumerable.Empty<IEdge<TNodeData, TEdgeData>>();
        }
    }
}
