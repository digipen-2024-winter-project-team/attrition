using System.Collections.Generic;

namespace Attrition.Common.Graphing
{
    public interface IGraphEnumerator
    {
        IEnumerable<INode<TNodeData, TEdgeData>> Enumerate<TNodeData, TEdgeData, TNode>(
            IGraph<TNodeData, TEdgeData> graph,
            TNode? startNode = null)
            where TNode : class, INode<TNodeData, TEdgeData>;
    }
}
