using System.Collections.Generic;

namespace Attrition.Common.Graphing
{
    public interface IAdjacencyList<TNodeData, TEdgeData>
    {
        IEnumerable<INode<TNodeData, TEdgeData>> Nodes { get; }
        IEnumerable<IEdge<TNodeData, TEdgeData>> Edges { get; }
        void AddNode(INode<TNodeData, TEdgeData> node);
        void RemoveNode(INode<TNodeData, TEdgeData> node);
        void AddEdge(IEdge<TNodeData, TEdgeData> edge);
        void RemoveEdge(IEdge<TNodeData, TEdgeData> edge);
        void RemoveAllEdgesFrom(INode<TNodeData, TEdgeData> node);
        IEnumerable<IEdge<TNodeData, TEdgeData>> GetEdgesFrom(INode<TNodeData, TEdgeData> node);
        void Clear();
    }
}
