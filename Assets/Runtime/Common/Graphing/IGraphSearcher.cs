using System.Collections.Generic;

namespace Attrition.Common.Graphing
{
    public interface IGraphSearcher<TNodeData, TEdgeData>
    {
        IEnumerable<INode<TNodeData, TEdgeData>> Search(IGraph<TNodeData, TEdgeData> graph, INode<TNodeData, TEdgeData> from, INode<TNodeData, TEdgeData> to);
    }
}