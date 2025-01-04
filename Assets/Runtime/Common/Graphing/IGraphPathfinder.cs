using System;
using System.Collections.Generic;

namespace Attrition.Common.Graphing
{
    public interface IGraphPathfinder<TNodeData, TEdgeData>
    {
        IEnumerable<TNode> FindPath<TNode>(TNode from, TNode to)
            where TNode : class, INode<TNodeData, TEdgeData>;
    }
}
