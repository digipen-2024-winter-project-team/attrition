using Attrition.Common.Graphing;

namespace Attrition.Ecosystem
{
    public class DungeonGraph : UndirectedGraph<DungeonRoom, float>
    {
        
    }

    public class DungeonRoomNode : Node<DungeonRoom, float>
    {
        public DungeonRoomNode(DungeonGraph graph, DungeonRoom value)
            : base(graph, value) { }
    }

    public class DungeonEdge : Edge<DungeonRoom, float>
    {
        public DungeonEdge(DungeonGraph graph, DungeonRoomNode from, DungeonRoomNode to, float cost = default) 
            : base(graph, from, to, cost) { }
    }
}
