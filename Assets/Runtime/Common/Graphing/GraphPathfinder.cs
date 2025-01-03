using System;
using System.Collections.Generic;

namespace Attrition.Common.Graphing
{
    public abstract class GraphPathfinder<TNodeData, TEdgeData> : IGraphPathfinder<TNodeData, TEdgeData>
    {
        private readonly IGraph<TNodeData, TEdgeData> graph;
        private readonly IGraphEnumerator enumerator;
        private readonly Func<INode<TNodeData, TEdgeData>, INode<TNodeData, TEdgeData>, float> heuristic;

        public GraphPathfinder(
            IGraph<TNodeData, TEdgeData> graph,
            IGraphEnumerator enumerator)
        {
            this.graph = graph;
            this.enumerator = enumerator;
        }

        public abstract IEnumerable<TNode> FindPath<TNode>(TNode from, TNode to)
            where TNode : class, INode<TNodeData, TEdgeData>;
    }
}

