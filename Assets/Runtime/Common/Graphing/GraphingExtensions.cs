using System;

namespace Attrition.Common.Graphing
{
    public static class GraphingExtensions
    {
        public static INode<TNodeData, TEdgeData> AddNode<TGraph, TNodeData, TEdgeData>(
            this TGraph graph, 
            TNodeData value)
            where TGraph : IGraph<TNodeData, TEdgeData>
        {
            var node = new Node<TNodeData, TEdgeData>(graph, value);
            return graph.AddNode(node);
        }
        
        public static IEdge<TNodeData, TEdgeData> AddEdge<TGraph, TNodeData, TEdgeData>(
            this TGraph graph,
            INode<TNodeData, TEdgeData> source, 
            INode<TNodeData, TEdgeData> target, 
            TEdgeData value = default)
            where TGraph : IGraph<TNodeData, TEdgeData>
        {
            var edge = new Edge<TNodeData, TEdgeData>(graph, source, target, value);
            return graph.AddEdge(edge);
        }
    }
}
