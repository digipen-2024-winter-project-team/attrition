using System.Collections.Generic;

namespace Attrition.Common.Graphing
{
    public interface IGraph<TNodeData, TEdgeData>
    {
        IEnumerable<INode<TNodeData, TEdgeData>> Nodes { get; }
        
        IEnumerable<IEdge<TNodeData, TEdgeData>> Edges { get; }
        
        TNode AddNode<TNode>(TNode node)
            where TNode : INode<TNodeData, TEdgeData>;
        
        void RemoveNode<TNode>(TNode node)
            where TNode : INode<TNodeData, TEdgeData>;
        
        TEdge AddEdge<TEdge>(TEdge edge)
            where TEdge : IEdge<TNodeData, TEdgeData>;
        
        void RemoveEdge<TEdge>(TEdge edge)
            where TEdge : IEdge<TNodeData, TEdgeData>;
        
        void Clear();
    }
}
