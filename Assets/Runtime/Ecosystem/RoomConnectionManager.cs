using System;
using System.Linq;
using UnityEngine;
using Attrition.Common.Graphing;
using System.Collections.Generic;
using Attrition.Common;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Attrition.Ecosystem
{
    public class RoomConnectionManager : MonoBehaviour
    {
        [SerializeField] private float maxConnectionDistance;
        
        private Room[] rooms;
        
        #region Gizmos
        
        public const string GizmoVisibilityKey = "RoomConnectionManagerGizmoVisiblity";

        private static GizmoVisibility GizmoVisibility => 
#if UNITY_EDITOR
            (GizmoVisibility)EditorPrefs.GetInt(GizmoVisibilityKey, 0);
#else
            GizmoVisibility.NeverShow;
#endif
        
        private void OnDrawGizmos()
        {
            if (GizmoVisibility == GizmoVisibility.AlwaysShow)
            {
                DrawGizmos();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (GizmoVisibility == GizmoVisibility.ShowWhenSelected)
            {
                DrawGizmos();
            }
        }

        private void DrawGizmos()
        {
            BuildRoomConnections();
            
            Gizmos.color = Color.red;
            
            foreach (var room in rooms)
            {
                foreach (var point in room.points
                             .Where(point => point.connection != null))
                {
                    Gizmos.DrawLine(point.Position, point.connection.Position);
                }
            }
        }
        
        #endregion

        private class Room
        {
            public Room(DungeonRoom sceneReference)
            {
                this.sceneReference = sceneReference;
                points = sceneReference.GetComponentsInChildren<RoomConnectionPoint>()
                    .Select(point => new Point(this, point))
                    .ToList();
            }
            
            public readonly DungeonRoom sceneReference;
            public readonly List<Point> points;
        }

        private class Point
        {
            public Point(Room room, RoomConnectionPoint sceneReference)
            {
                this.room = room;
                this.sceneReference = sceneReference;
            }

            public readonly Room room;
            private readonly RoomConnectionPoint sceneReference;
            public Vector3 Position => sceneReference.transform.position;
            
            public Point connection;
            
            public float DistanceTo(Point other) =>
                Vector3.Distance(Position, other.Position);
            
            public void ConnectTo(Point point)
            {
                connection = point;
                point.connection = this;
            }
        }
        
        private void BuildRoomConnections()
        {
            rooms = GetComponentsInChildren<DungeonRoom>()
                .Select(room => new Room(room))
                .ToArray();

            var allPoints = rooms.SelectMany(room => room.points).ToArray();
            var unresolvedPoints = allPoints.ToList();

            // max iterations to avoid infinite loops or too many resolving iterations
            int maxIterations = 100;
            
            // continue iterating until every point has been resolved
            // collisions will force points to be resolved again with their next closest point
            while (unresolvedPoints.Count > 0)
            {
                maxIterations--;
                if (maxIterations < 0)
                {
                    Debug.LogError("Building room points went beyond max iterations!");
                    break;
                }
                
                var resolving = unresolvedPoints[0];

                // get all points from other rooms, filtering out ones that are too far and sorting by distance
                var closestPoints = rooms
                    .Where(room => room != resolving.room)
                    .SelectMany(room => room.points)
                    .Select(point => (point, distance: resolving.DistanceTo(point)))
                    .Where(candidate => candidate.distance <= maxConnectionDistance)
                    .OrderBy(candidate => candidate.distance)
                    .ToArray();
                
                foreach ((var closest, float distance) in closestPoints)
                {
                    // checking if any points to this point already exist
                    var conflict = allPoints
                        .FirstOrDefault(point => point.connection == closest && point != resolving);
                    
                    // no conflict
                    if (conflict == null)
                    {
                        resolving.ConnectTo(closest);
                        break;
                    }
                    
                    // resolving point is closer, and overrides the conflict
                    else if (distance < conflict.DistanceTo(closest))
                    {
                        resolving.ConnectTo(closest);
                        
                        conflict.connection = null;
                        unresolvedPoints.Add(conflict);
                        
                        break;
                    }

                    // conflict could not be overriden, try next point
                }
                
                // point has been resolved
                unresolvedPoints.Remove(resolving);
            }
        }

        private void BuildGraph()
        {
            var graph = new DungeonGraph();

            foreach (var room in rooms)
            {
                graph.AddNode(new DungeonRoomNode(graph, room.sceneReference));
            }

            foreach (var room in rooms)
            {
                var from = (DungeonRoomNode)graph.Nodes
                    .First(node => node.Value == room.sceneReference);
                
                foreach (var point in room.points)
                {
                    var to = (DungeonRoomNode)graph.Nodes
                        .First(node => node.Value == point.connection.room.sceneReference);

                    graph.AddEdge(new DungeonEdge(graph, from, to));
                }
            }
        }
    }
}
