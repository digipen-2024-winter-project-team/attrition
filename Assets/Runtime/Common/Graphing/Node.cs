namespace Attrition.Common.Graphing
{
    public class Node<TNodeData, TEdgeData> : INode<TNodeData, TEdgeData>
    {
        public IGraph<TNodeData, TEdgeData> Graph { get; }
        public TNodeData Value { get; }
        
        public Node(IGraph<TNodeData, TEdgeData> graph, TNodeData value)
        {
            this.Graph = graph;
            this.Value = value;
        }
    }
}