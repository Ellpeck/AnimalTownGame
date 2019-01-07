using System;
using System.Collections.Generic;
using AnimalTownGame.Maps;
using AnimalTownGame.Objects;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Misc {
    public static class PathFinding {

        public const int DefaultPathfindCost = 1000;

        public static Stack<Point> FindPath(Map map, DynamicObject obj, Point start, Point goal, int maxTries) {
            var open = new HashSet<PathPoint>();
            var closed = new HashSet<PathPoint>();
            open.Add(new PathPoint(start, goal, null, 0));

            var count = 0;
            while (open.Count > 0) {
                PathPoint current = null;
                var lowestF = int.MaxValue;
                foreach (var point in open)
                    if (point.F < lowestF) {
                        current = point;
                        lowestF = point.F;
                    }
                if (current == null)
                    break;

                open.Remove(current);
                closed.Add(current);

                if (current.Pos.Equals(goal))
                    return CompilePath(current);

                foreach (var dir in Direction.Adjacents) {
                    var neighborPos = current.Pos + dir.Offset;
                    var cost = GetCost(map, neighborPos, obj);
                    if (cost < int.MaxValue) {
                        var neighbor = new PathPoint(neighborPos, goal, current, cost);
                        if (!closed.Contains(neighbor)) {
                            PathPoint alreadyNeighbor;
                            open.TryGetValue(neighbor, out alreadyNeighbor);
                            if (alreadyNeighbor == null)
                                open.Add(neighbor);
                            else if (neighbor.G < alreadyNeighbor.G) {
                                open.Remove(alreadyNeighbor);
                                open.Add(neighbor);
                            }
                        }
                    }
                }

                count++;
                if (count >= maxTries)
                    break;
            }
            return null;
        }

        private static Stack<Point> CompilePath(PathPoint current) {
            var path = new Stack<Point>();
            while (current != null) {
                path.Push(current.Pos);
                current = current.Parent;
            }
            return path;
        }

        private static int GetCost(Map map, Point point, DynamicObject obj) {
            if (!map.IsInBounds(point.X, point.Y))
                return int.MaxValue;
            var tile = map[point];
            if (tile != null)
                return tile.Type.PathCost;
            return DefaultPathfindCost;
        }

    }

    public class PathPoint {

        public readonly PathPoint Parent;
        public readonly Point Pos;
        public readonly int F;
        public readonly int G;

        public PathPoint(Point pos, Point goal, PathPoint parent, int terrainCostForThisPos) {
            this.Pos = pos;
            this.Parent = parent;

            this.G = (parent == null ? 0 : parent.G) + terrainCostForThisPos;
            var manhattan = (Math.Abs(goal.X - pos.X) + Math.Abs(goal.Y - pos.Y)) * PathFinding.DefaultPathfindCost;
            this.F = this.G + manhattan;
        }

        public override bool Equals(object obj) {
            if (obj == this) {
                return true;
            }
            var point = obj as PathPoint;
            return point != null && point.Pos.Equals(this.Pos);
        }

        public override int GetHashCode() {
            return this.Pos.GetHashCode();
        }

    }
}