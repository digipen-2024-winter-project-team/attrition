namespace Attrition.Common.Graphing
{
    public interface IEdge<TNodeData, TEdgeData>
    {
        IGraph<TNodeData, TEdgeData> Graph { get; }
        INode<TNodeData, TEdgeData> From { get; }
        INode<TNodeData, TEdgeData> To { get; }
        TEdgeData Cost { get; }
    }
}