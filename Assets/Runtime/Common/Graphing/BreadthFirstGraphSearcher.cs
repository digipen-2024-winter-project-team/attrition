using System;
using System.Collections.Generic;
using System.Linq;

namespace Attrition.Common.Graphing
{
    public class BreadthFirstGraphSearcher<TNodeData, TEdgeData> : IGraphSearcher<TNodeData, TEdgeData>
    {
        private readonly IGraph<TNodeData, TEdgeData> graph;

        public BreadthFirstGraphSearcher(IGraph<TNodeData, TEdgeData> graph)
        {
            this.graph = graph;
        }
        
        public IEnumerable<INode<TNodeData, TEdgeData>> Search<TNode>(TNode from, TNode to)
            where TNode : INode<TNodeData, TEdgeData>
        {
            if (!this.graph.Nodes.Contains(from) || !this.graph.Nodes.Contains(to))
            {
                throw new GraphSearchException<TNodeData, TEdgeData>(this.graph, from, to, 
                    $"Both {nameof(from)} and {nameof(to)} nodes must be contained in graph {this.graph}.");
            }
            
            if (from == null || to == null)
            {
                yield break;
            }
            
            if (from.Equals(to))
            {
                yield return from;
                yield break;
            }
            
            if (!this.graph.Nodes.Any() || !this.graph.Edges.Any())
            {
                yield break;
            }

            var visited = new HashSet<INode<TNodeData, TEdgeData>>();
            var queue = new Queue<INode<TNodeData, TEdgeData>>();
            var parents = new Dictionary<INode<TNodeData, TEdgeData>, INode<TNodeData, TEdgeData>>();

            queue.Enqueue(from);
            visited.Add(from);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                yield return current;

                if (current.Equals(to))
                {
                    break;
                }

                foreach (var neighbor in current.GetNeighbors())
                {
                    if (!visited.Add(neighbor))
                    {
                        continue;
                    }

                    queue.Enqueue(neighbor);
                    parents[neighbor] = current;
                }
            }

            // Optionally, reconstruct the path from `from` to `to` (if needed).
            if (visited.Contains(to))
            {
                var path = new Stack<INode<TNodeData, TEdgeData>>();
                var current = to as INode<TNodeData, TEdgeData>;

                while (current != null)
                {
                    path.Push(current);
                    parents.TryGetValue(current, out var parent);
                    current = parent;
                }

                foreach (var node in path)
                {
                    yield return node;
                }
            }
        }
    }
}
