using System;
using System.Collections.Generic;

namespace Attrition.Common.Graphing
{
    public class GraphSearcher<TNodeData, TEdgeData> : IGraphSearcher<TNodeData, TEdgeData>
    {
        private readonly IGraph<TNodeData, TEdgeData> graph;
        
        public GraphSearcher(IGraph<TNodeData, TEdgeData> graph)
        {
            this.graph = graph;
        }
        
        public IEnumerable<INode<TNodeData, TEdgeData>> Search(IGraph<TNodeData, TEdgeData> graph, INode<TNodeData, TEdgeData> from, INode<TNodeData, TEdgeData> to)
        {
            throw new NotImplementedException();
        }
    }
}