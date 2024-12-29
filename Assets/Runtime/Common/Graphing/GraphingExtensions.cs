using System;

namespace Attrition.Common.Graphing
{
    public static class GraphingExtensions
    {
        public static void AddNode<TNodeData>(this IGraph<TNodeData, int> graph, TNodeData value)
        {
            graph.AddNode(value);
        }
        
        public static INode<TNodeData, TEdgeData> GetNeighbor<TNodeData, TEdgeData>(this INode<TNodeData, TEdgeData> node)
        {
            throw new NotImplementedException();
        }
        
        public static INode<TNodeData, TEdgeData> AddNeighbor<TNodeData, TEdgeData>()
        {
            throw new NotImplementedException();
        }
    }
}