using System;
using System.Collections.Generic;
using UnityEngine;

namespace Attrition.Common.Graphing
{
    public static class GraphingExtensions
    {
        public static void AddNode<TNodeData>(this IGraph<TNodeData, int> graph, TNodeData value)
        {
            graph.AddNode(value);
        }
        
        public static INode<TNodeData, TEdgeData> GetNeighbor<TNodeData, TEdgeData>(this INode<TNodeData, TEdgeData> node)
        {
            throw new NotImplementedException();
        }
        
        public static INode<TNodeData, TEdgeData> AddNeighbor<TNodeData, TEdgeData>()
        {
            throw new NotImplementedException();
        }
    }
    
    public interface IGraph<TNodeData, TEdgeData>
    {
        IEnumerable<INode<TNodeData, TEdgeData>> Nodes { get; }
        IEnumerable<IEdge<TNodeData, TEdgeData>> Edges { get; }
        
        INode<TNodeData, TEdgeData> AddNode(TNodeData value);
        void RemoveNode(INode<TNodeData, TEdgeData> node);
        IEdge<TNodeData, TEdgeData> AddEdge(INode<TNodeData, TEdgeData> from, INode<TNodeData, TEdgeData> to, TEdgeData value = default);
        void RemoveEdge(IEdge<TNodeData, TEdgeData> edge);
        
        void Clear();
    }
    
    public interface INode<TNodeData, TEdgeData>
    {
        IGraph<TNodeData, TEdgeData> Graph { get; }
        TNodeData Value { get; }
    }
    
    public interface IEdge<TNodeData, TEdgeData>
    {
        IGraph<TNodeData, TEdgeData> Graph { get; }
        INode<TNodeData, TEdgeData> From { get; }
        INode<TNodeData, TEdgeData> To { get; }
        TEdgeData Cost { get; }
    }
    
    public interface IGraphSearcher<TNodeData, TEdgeData>
    {
        IEnumerable<INode<TNodeData, TEdgeData>> Search(IGraph<TNodeData, TEdgeData> graph, INode<TNodeData, TEdgeData> from, INode<TNodeData, TEdgeData> to);
    }
    
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

    public class Sample
    {
        public Sample()
        {
            var graph = new Graph<string, float>();
            
            var foo = new Node<string, float>(graph, "foo");
            var bar = new Node<string, float>(graph, "bar");
            var baz = new Node<string, float>(graph, "baz");
            var foobar = new Edge<string, float>(graph, foo, bar);
            var barbaz = new Edge<string, float>(graph, bar, baz);
            
            var searcher = new GraphSearcher<string, float>(graph);
            
            foreach (var node in searcher.Search(graph, foo, baz))
            {
                Debug.Log(node.Value);
            }
        }
    }

    public class Graph<TNodeData, TEdgeData> : IGraph<TNodeData, TEdgeData>
    {
        public IEnumerable<INode<TNodeData, TEdgeData>> Nodes { get; }
        public IEnumerable<IEdge<TNodeData, TEdgeData>> Edges { get; }
        
        public INode<TNodeData, TEdgeData> AddNode(TNodeData value)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveNode(INode<TNodeData, TEdgeData> node)
        {
            throw new System.NotImplementedException();
        }

        public IEdge<TNodeData, TEdgeData> AddEdge(INode<TNodeData, TEdgeData> from, INode<TNodeData, TEdgeData> to, TEdgeData value = default)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEdge(IEdge<TNodeData, TEdgeData> edge)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class Node<TNodeData, TEdgeData> : INode<TNodeData, TEdgeData>
    {
        public IGraph<TNodeData, TEdgeData> Graph { get; }
        public TNodeData Value { get; }
        
        public Node(IGraph<TNodeData, TEdgeData> graph, TNodeData value)
        {
            this.Graph = graph;
            this.Value = value;
        }
    }
    
    public class Edge<TNodeData, TEdgeData> : IEdge<TNodeData, TEdgeData>
    {
        public IGraph<TNodeData, TEdgeData> Graph { get; }
        public INode<TNodeData, TEdgeData> From { get; }
        public INode<TNodeData, TEdgeData> To { get; }
        public TEdgeData Cost { get; }
        
        public Edge(IGraph<TNodeData, TEdgeData> graph, INode<TNodeData, TEdgeData> from, INode<TNodeData, TEdgeData> to, TEdgeData cost = default)
        {
            this.Graph = graph;
            this.From = from;
            this.To = to;
            this.Cost = cost;
        }
    }
}
