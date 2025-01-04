using System;
using System.Collections.Generic;
using System.Linq;

namespace Attrition.Common.Graphing
{
    public class BreadthFirstGraphEnumerator : IGraphEnumerator
    {
        public IEnumerable<INode<TNodeData, TEdgeData>> Enumerate<TNodeData, TEdgeData, TNode>(
            IGraph<TNodeData, TEdgeData> graph,
            TNode? startNode = null)
            where TNode : class, INode<TNodeData, TEdgeData>
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph), "Graph cannot be null.");
            }

            if (!graph.Nodes.Any())
            {
                yield break; // No nodes in the graph
            }

            var visited = new HashSet<INode<TNodeData, TEdgeData>>();
            var queue = new Queue<INode<TNodeData, TEdgeData>>();

            // Determine starting nodes
            var startingNodes = startNode != null
                ? new[] { startNode }
                : graph.Nodes.OfType<TNode>();

            foreach (var node in startingNodes)
            {
                if (startNode != null && !graph.Nodes.Contains(node))
                {
                    throw new ArgumentException("Start node must be contained in the graph.", nameof(startNode));
                }

                if (visited.Contains(node))
                {
                    continue; // Skip already visited nodes
                }

                queue.Enqueue(node);
                visited.Add(node);

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    yield return current;

                    foreach (var neighbor in current.GetNeighbors())
                    {
                        if (visited.Add(neighbor))
                        {
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }
        }
    }
}
