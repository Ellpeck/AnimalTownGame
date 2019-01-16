using System;
using System.Collections.Generic;
using System.Linq;
using AnimalTownGame.Misc;
using AnimalTownGame.Objects;
using AnimalTownGame.Objects.Static;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AnimalTownGame.Maps {
    public class Map {

        public readonly string Name;
        public readonly int WidthInTiles;
        public readonly int HeightInTiles;
        public readonly bool IsInside;
        public readonly bool CanHaveFurniture;
        public readonly Vector2 EntryPoint;
        private readonly Tile[,] tileGrid;

        public readonly List<DynamicObject> DynamicObjects = new List<DynamicObject>();
        public readonly List<StaticObject> StaticObjects = new List<StaticObject>();
        public IEnumerable<MapObject> AllObjects => this.DynamicObjects.Concat<MapObject>(this.StaticObjects);

        public Tile this[Point point] => this[point.X, point.Y];

        public Tile this[int x, int y] => this.IsInBounds(x, y) ? this.tileGrid[x, y] : null;

        public readonly Random Random = new Random();
        public int Ticks { get; private set; }

        public Map(string name, int widthInTiles, int heightInTiles, bool isInside, bool canHaveFurniture, Vector2 entryPoint = default(Vector2)) {
            this.Name = name;
            this.WidthInTiles = widthInTiles;
            this.HeightInTiles = heightInTiles;
            this.IsInside = isInside;
            this.CanHaveFurniture = canHaveFurniture;
            this.EntryPoint = entryPoint;
            this.tileGrid = new Tile[widthInTiles, heightInTiles];
        }

        public void AddObject(MapObject obj) {
            var stat = obj as StaticObject;
            if (stat != null)
                this.StaticObjects.Add(stat);
            else
                this.DynamicObjects.Add((DynamicObject) obj);
        }

        public virtual void UpdateRealTime(DateTime now, DateTime lastUpdate, TimeSpan passed) {
            foreach (var obj in this.AllObjects)
                obj.UpdateRealTime(now, lastUpdate, passed);
        }

        public void Update(GameTime gameTime, bool isCurrent) {
            this.Ticks++;

            foreach (var obj in this.DynamicObjects)
                obj.Update(gameTime, isCurrent);
        }

        public void SetTile(Point point, TileType type) {
            if (!this.IsInBounds(point.X, point.Y))
                return;

            this.tileGrid[point.X, point.Y] = type.Instance(this, point);

            foreach (var dir in Direction.Arounds) {
                var tile = this[point + dir.Offset];
                if (tile != null)
                    tile.OnNeighborChanged(point);
            }
        }

        public bool IsInBounds(int x, int y) {
            return x >= 0 && y >= 0 && x < this.WidthInTiles && y < this.HeightInTiles;
        }

        public IEnumerable<MapObject> GetObjectsInArea(RectangleF area, BoundSelector boundSelector, ObjectSelector objSelector = null) {
            foreach (var obj in this.AllObjects) {
                if (objSelector != null && !objSelector(obj))
                    continue;
                var bound = boundSelector(obj).Move(obj.Position);
                if (area.IntersectsNonEmpty(bound))
                    yield return obj;
            }
        }

        public IEnumerable<Tile> GetTilesInArea(RectangleF area, TileSelector selector = null) {
            for (var x = 0; x <= area.Width.Ceil(); x++)
                for (var y = 0; y <= area.Height.Ceil(); y++) {
                    var tile = this[area.X.Floor() + x, area.Y.Floor() + y];
                    if (tile != null && (selector == null || selector(tile)))
                        yield return tile;
                }
        }

        public delegate bool ObjectSelector(MapObject obj);

        public delegate bool TileSelector(Tile tile);

        public delegate RectangleF BoundSelector(MapObject obj);

    }
}