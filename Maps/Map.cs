using System;
using System.Collections.Generic;
using AnimalTownGame.Misc;
using AnimalTownGame.Objects;
using AnimalTownGame.Objects.Static;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Maps {
    public class Map {

        public readonly string Name;
        public readonly int WidthInTiles;
        public readonly int HeightInTiles;
        public readonly bool IsInside;
        private readonly Tile[,] tileGrid;

        public readonly List<DynamicObject> DynamicObjects = new List<DynamicObject>();
        public readonly List<StaticObject> StaticObjects = new List<StaticObject>();

        public Tile this[Point point] => this[point.X, point.Y];

        public Tile this[int x, int y] => this.IsInBounds(x, y) ? this.tileGrid[x, y] : null;

        public readonly Random Random = new Random();
        public int Ticks { get; private set; }

        public Map(string name, int widthInTiles, int heightInTiles, bool isInside) {
            this.Name = name;
            this.WidthInTiles = widthInTiles;
            this.HeightInTiles = heightInTiles;
            this.IsInside = isInside;
            this.tileGrid = new Tile[widthInTiles, heightInTiles];
        }

        public virtual void UpdateRealTime(DateTime now, DateTime lastUpdate, TimeSpan passed) {
            foreach (var obj in this.DynamicObjects)
                obj.UpdateRealTime(now, lastUpdate, passed);
            foreach (var obj in this.StaticObjects)
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

            foreach (var dir in Direction.Values) {
                var tile = this[point + dir.Offset];
                if (tile != null)
                    tile.OnNeighborChanged(point);
            }
        }

        public bool IsInBounds(int x, int y) {
            return x >= 0 && y >= 0 && x < this.WidthInTiles && y < this.HeightInTiles;
        }

    }
}