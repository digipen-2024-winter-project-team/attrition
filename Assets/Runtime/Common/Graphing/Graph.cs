using System;
using System.Collections.Generic;

namespace Attrition.Common.Graphing
{
    public abstract class Graph<TNodeData, TEdgeData> : IGraph<TNodeData, TEdgeData>
    {
        private readonly IAdjacencyList<TNodeData, TEdgeData> adjacencies;
        
        public IEnumerable<INode<TNodeData, TEdgeData>> Nodes => this.adjacencies.Nodes;
        public IEnumerable<IEdge<TNodeData, TEdgeData>> Edges => this.adjacencies.Edges;
        
        protected Graph()
        {
            this.adjacencies = new AdjacencyList<TNodeData, TEdgeData>();
        }
        
        protected Graph(IAdjacencyList<TNodeData, TEdgeData> adjacencies)
        {
            this.adjacencies = adjacencies;
        }
        
        public virtual INode<TNodeData, TEdgeData> AddNode(TNodeData value)
        {
            var node = new Node<TNodeData, TEdgeData>(this, value);
            this.adjacencies.AddNode(node);
            return node;
        }

        public virtual INode<TNodeData, TEdgeData> AddNode(INode<TNodeData, TEdgeData> node)
        {
            this.adjacencies.AddNode(node);
            return node;
        }

        public virtual void RemoveNode(INode<TNodeData, TEdgeData> node)
        {
            this.adjacencies.RemoveNode(node);
        }

        public virtual IEdge<TNodeData, TEdgeData> AddEdge(INode<TNodeData, TEdgeData> from, INode<TNodeData, TEdgeData> to, TEdgeData value = default)
        {
            var edge = new Edge<TNodeData, TEdgeData>(this, from, to, value);
            return this.AddEdge(edge);
        }

        public IEdge<TNodeData, TEdgeData> AddEdge(IEdge<TNodeData, TEdgeData> edge)
        {
            this.adjacencies.AddEdge(edge);
            return edge;
        }

        public virtual void RemoveEdge(IEdge<TNodeData, TEdgeData> edge)
        {
            this.adjacencies.RemoveEdge(edge);
        }

        public virtual void Clear()
        {
            this.adjacencies.Clear();
        }
    }
}
