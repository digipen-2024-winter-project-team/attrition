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
        
        public IEnumerable<INode<TNodeData, TEdgeData>> Search<TNode>(TNode from, TNode to) 
            where TNode : INode<TNodeData, TEdgeData> 
        {
            throw new NotImplementedException();
        }
    }
}
