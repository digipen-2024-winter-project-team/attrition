using System.Collections.Generic;
using System.Linq;

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
            throw new System.NotImplementedException();
        }

        public virtual IEdge<TNodeData, TEdgeData> AddEdge(INode<TNodeData, TEdgeData> from, INode<TNodeData, TEdgeData> to, TEdgeData value = default)
        {
            throw new System.NotImplementedException();
        }

        public virtual void RemoveEdge(IEdge<TNodeData, TEdgeData> edge)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}
