using System;
using System.Collections.Generic;
using AnimalTownGame.Maps.Objects;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Maps {
    public class Map {

        public readonly string Name;
        public readonly int WidthInTiles;
        public readonly int HeightInTiles;
        private readonly Tile[,] tileGrid;

        public readonly List<DynamicObject> DynamicObjects = new List<DynamicObject>();

        public Tile this[int x, int y] => this.tileGrid[x, y];

        public Map(string name, int widthInTiles, int heightInTiles) {
            this.Name = name;
            this.WidthInTiles = widthInTiles;
            this.HeightInTiles = heightInTiles;
            this.tileGrid = new Tile[widthInTiles, heightInTiles];
        }

        public void Update(TimeSpan passed) {
            foreach (var obj in this.DynamicObjects)
                obj.Update(passed);
        }

        public void SetTile(int x, int y, TileType type) {
            this.tileGrid[x, y] = type.Instance(this, new Point(x, y));
        }

    }
}