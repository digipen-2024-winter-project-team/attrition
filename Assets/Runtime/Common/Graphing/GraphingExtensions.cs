using System.Collections.Generic;
using System.Linq;

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

        public static IEnumerable<INode<TNodeData, TEdgeData>> GetNeighbors<TNodeData, TEdgeData>(
            this INode<TNodeData, TEdgeData> node)
        {
            return node.Graph.GetEdges(node)
                .Where(edge => edge.From == node || edge.To == node)
                .Select(edge => edge.From == node ? edge.To : edge.From);
        }
    }
}
