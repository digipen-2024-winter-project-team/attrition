namespace Attrition.Common.Graphing
{
    public interface INode<TNodeData, TEdgeData>
    {
        IGraph<TNodeData, TEdgeData> Graph { get; }
        TNodeData Value { get; }
    }
}
