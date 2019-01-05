using System;

namespace AnimalTownGame.Maps {
    public class Map {

        public readonly string Name;
        public readonly int WidthInTiles;
        public readonly int HeightInTiles;
        private readonly Tile[,] tileGrid;

        public Tile this[int x, int y] {
            get { return this.tileGrid[x, y]; }
            set { this.tileGrid[x, y] = value; }
        }

        public Map(string name, int widthInTiles, int heightInTiles) {
            this.Name = name;
            this.WidthInTiles = widthInTiles;
            this.HeightInTiles = heightInTiles;
            this.tileGrid = new Tile[widthInTiles, heightInTiles];
        }

        public void Update(TimeSpan passed) {
        }

    }
}