using System.Collections.Generic;

namespace Attrition.Common.Graphing
{
    public interface IGraphSearcher<TNodeData, TEdgeData>
    {
        IEnumerable<INode<TNodeData, TEdgeData>> Search<TNode>(TNode from, TNode to)
            where TNode : INode<TNodeData, TEdgeData>;
    }
}
