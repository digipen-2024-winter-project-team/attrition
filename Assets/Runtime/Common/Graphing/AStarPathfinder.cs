using System;
using System.Collections.Generic;
using System.Linq;

namespace Attrition.Common.Graphing
{
    public class AStarPathfinder<TNodeData, TEdgeData> : GraphPathfinder<TNodeData, TEdgeData>
    {
        private readonly IGraph<TNodeData, TEdgeData> graph;
        private readonly IGraphEnumerator enumerator;
        private readonly Func<INode<TNodeData, TEdgeData>, INode<TNodeData, TEdgeData>, float> heuristic;

        public AStarPathfinder(
            IGraph<TNodeData, TEdgeData> graph,
            IGraphEnumerator enumerator,
            Func<INode<TNodeData, TEdgeData>, INode<TNodeData, TEdgeData>, float> heuristic)
            : base(graph, enumerator)
        {
            this.graph = graph ?? throw new ArgumentNullException(nameof(graph));
            this.enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
            this.heuristic = heuristic ?? throw new ArgumentNullException(nameof(heuristic));
        }

        public override IEnumerable<TNode> FindPath<TNode>(TNode from, TNode to)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            if (!this.graph.Nodes.Contains(from) || !this.graph.Nodes.Contains(to))
            {
                throw new ArgumentException("Both 'from' and 'to' nodes must exist in the graph.");
            }

            // Priority queue for open set
            var openSet = new PriorityQueue<TNode>();
            openSet.Enqueue(from, 0);

            // Path reconstruction map
            var cameFrom = new Dictionary<TNode, TNode>();

            // Cost maps
            var gScore = new Dictionary<TNode, float> { [from] = 0 };
            var fScore = new Dictionary<TNode, float> { [from] = this.heuristic(from, to) };

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                if (current.Equals(to))
                {
                    return this.ReconstructPath(cameFrom, current);
                }

                // Enumerate only direct neighbors of the current node
                foreach (var neighbor in current.GetNeighbors()
                             .OfType<TNode>())
                {
                    var tentativeGScore = gScore[current] + this.GetEdgeCost(current, neighbor);

                    if (!gScore.TryGetValue(neighbor, out var neighborGScore) || tentativeGScore < neighborGScore)
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + this.heuristic(neighbor, to);

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Enqueue(neighbor, fScore[neighbor]);
                        }
                    }
                }
            }

            // No path found
            return Enumerable.Empty<TNode>();
        }


        private IEnumerable<TNode> ReconstructPath<TNode>(Dictionary<TNode, TNode> cameFrom, TNode current)
        {
            var path = new List<TNode> { current };

            while (cameFrom.TryGetValue(current, out var previous))
            {
                current = previous;
                path.Add(current);
            }

            path.Reverse();
            return path;
        }

        private float GetEdgeCost<TNode>(TNode from, TNode to)
            where TNode : INode<TNodeData, TEdgeData>
        {
            // Implement actual edge cost calculation logic here.
            // Example assumes uniform edge costs of 1.
            return 1;
        }
    }

    public class PriorityQueue<T>
    {
        private readonly SortedDictionary<float, Queue<T>> elements = new();

        public int Count { get; private set; }

        public void Enqueue(T item, float priority)
        {
            if (!this.elements.TryGetValue(priority, out var queue))
            {
                queue = new Queue<T>();
                this.elements[priority] = queue;
            }

            queue.Enqueue(item);
            this.Count++;
        }

        public T Dequeue()
        {
            if (this.elements.Count == 0)
            {
                throw new InvalidOperationException("The priority queue is empty.");
            }

            var firstPair = this.elements.First();
            var item = firstPair.Value.Dequeue();

            if (firstPair.Value.Count == 0)
            {
                this.elements.Remove(firstPair.Key);
            }

            this.Count--;
            return item;
        }

        public bool Contains(T item)
        {
            return this.elements.Values.Any(queue => queue.Contains(item));
        }
    }
}
