using System;
using System.Collections.Generic;
using AnimalTownGame.Maps.Objects;
using AnimalTownGame.Maps.Objects.Static;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Maps {
    public class Map {

        public readonly string Name;
        public readonly int WidthInTiles;
        public readonly int HeightInTiles;
        private readonly Tile[,] tileGrid;

        public readonly List<DynamicObject> DynamicObjects = new List<DynamicObject>();
        public readonly List<StaticObject> StaticObjects = new List<StaticObject>();

        public Tile this[Point point] => this[point.X, point.Y];

        public Tile this[int x, int y] {
            get {
                if (x < 0 || y < 0 || x >= this.WidthInTiles || y >= this.HeightInTiles)
                    return null;
                return this.tileGrid[x, y];
            }
        }

        public Map(string name, int widthInTiles, int heightInTiles) {
            this.Name = name;
            this.WidthInTiles = widthInTiles;
            this.HeightInTiles = heightInTiles;
            this.tileGrid = new Tile[widthInTiles, heightInTiles];
        }

        public virtual void UpdateRealTime(DateTime now, DateTime lastUpdate, TimeSpan passed) {
            foreach (var obj in this.DynamicObjects)
                obj.UpdateRealTime(now, lastUpdate, passed);
            foreach (var obj in this.StaticObjects)
                obj.UpdateRealTime(now, lastUpdate, passed);
        }

        public void Update(GameTime gameTime) {
            foreach (var obj in this.DynamicObjects)
                obj.Update(gameTime);
        }

        public void SetTile(Point point, TileType type) {
            this.tileGrid[point.X, point.Y] = type.Instance(this, point);

            foreach (var dir in Direction.Values) {
                var tile = this[point + dir.Offset];
                if (tile != null)
                    tile.OnNeighborChanged(point);
            }
        }

    }
}