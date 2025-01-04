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

        public IEnumerable<IEdge<TNodeData, TEdgeData>> GetEdges<TNode>(TNode node)
            where TNode : INode<TNodeData, TEdgeData>
        {
            return this.adjacencies.GetEdgesFrom(node);
        }
        
        public TNode AddNode<TNode>(TNode node)
            where TNode : INode<TNodeData, TEdgeData>
        {
            this.adjacencies.AddNode(node);
            return node;
        }

        public virtual void RemoveNode<TNode>(TNode node)
            where TNode : INode<TNodeData, TEdgeData>
        {
            this.adjacencies.RemoveNode(node);
        }

        public TEdge AddEdge<TEdge>(TEdge edge)
            where TEdge : IEdge<TNodeData, TEdgeData>
        {
            this.adjacencies.AddEdge(edge);
            return edge;
        }

        public virtual void RemoveEdge<TEdge>(TEdge edge)
            where TEdge : IEdge<TNodeData, TEdgeData>
        {
            this.adjacencies.RemoveEdge(edge);
        }

        public virtual void Clear()
        {
            this.adjacencies.Clear();
        }
    }
}
