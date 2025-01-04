using System;

namespace Attrition.Common.Graphing
{
    public class GraphSearchException<TNodeData, TEdgeData> : Exception
    {
        public IGraph<TNodeData, TEdgeData> Graph { get; }
        public INode<TNodeData, TEdgeData> From { get; }
        public INode<TNodeData, TEdgeData> To { get; }

        public GraphSearchException(
            IGraph<TNodeData, 
            TEdgeData> graph, INode<TNodeData, 
            TEdgeData> from, INode<TNodeData, TEdgeData> to,
            string message = default,
            Exception innerException = null)
            : base(message, innerException)
        {
            this.Graph = graph;
            this.From = from;
            this.To = to;
        }
    }
}
