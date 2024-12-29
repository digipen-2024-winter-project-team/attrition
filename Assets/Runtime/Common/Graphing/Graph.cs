using System.Collections.Generic;

namespace Attrition.Common.Graphing
{
    public abstract class Graph<TNodeData, TEdgeData> : IGraph<TNodeData, TEdgeData>
    {
        public IEnumerable<INode<TNodeData, TEdgeData>> Nodes { get; }
        public IEnumerable<IEdge<TNodeData, TEdgeData>> Edges { get; }
        
        public INode<TNodeData, TEdgeData> AddNode(TNodeData value)
        {
            throw new System.NotImplementedException();
        }

        public INode<TNodeData, TEdgeData> AddNode(INode<TNodeData, TEdgeData> node)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveNode(INode<TNodeData, TEdgeData> node)
        {
            throw new System.NotImplementedException();
        }

        public IEdge<TNodeData, TEdgeData> AddEdge(INode<TNodeData, TEdgeData> from, INode<TNodeData, TEdgeData> to, TEdgeData value = default)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEdge(IEdge<TNodeData, TEdgeData> edge)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}
