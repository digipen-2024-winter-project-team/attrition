namespace Attrition.Common.Graphing
{
    public class Edge<TNodeData, TEdgeData> : IEdge<TNodeData, TEdgeData>
    {
        public IGraph<TNodeData, TEdgeData> Graph { get; }
        public INode<TNodeData, TEdgeData> From { get; }
        public INode<TNodeData, TEdgeData> To { get; }
        public TEdgeData Cost { get; }
        
        public Edge(IGraph<TNodeData, TEdgeData> graph, INode<TNodeData, TEdgeData> from, INode<TNodeData, TEdgeData> to, TEdgeData cost = default)
        {
            this.Graph = graph;
            this.From = from;
            this.To = to;
            this.Cost = cost;
        }
    }
}
