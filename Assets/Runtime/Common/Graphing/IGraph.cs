using System.Collections.Generic;

namespace Attrition.Common.Graphing
{
    public interface IGraph<TNodeData, TEdgeData>
    {
        IEnumerable<INode<TNodeData, TEdgeData>> Nodes { get; }
        IEnumerable<IEdge<TNodeData, TEdgeData>> Edges { get; }
        
        INode<TNodeData, TEdgeData> AddNode(TNodeData value);
        INode<TNodeData, TEdgeData> AddNode(INode<TNodeData, TEdgeData> node);
        void RemoveNode(INode<TNodeData, TEdgeData> node);
        IEdge<TNodeData, TEdgeData> AddEdge(INode<TNodeData, TEdgeData> from, INode<TNodeData, TEdgeData> to, TEdgeData value = default);
        void RemoveEdge(IEdge<TNodeData, TEdgeData> edge);
        
        void Clear();
    }
}
