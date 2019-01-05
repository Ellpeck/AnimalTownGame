using Microsoft.Xna.Framework;

namespace AnimalTownGame.Maps {
    public class Tile {

        public readonly TileType type;
        public readonly Point Position;

        public Tile(TileType type, Point position) {
            this.type = type;
            this.Position = position;
        }

    }
}