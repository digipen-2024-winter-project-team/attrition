using System.Collections.Generic;

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
            if (from == null || to == null)
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
