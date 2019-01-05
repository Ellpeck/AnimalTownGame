using Microsoft.Xna.Framework;

namespace AnimalTownGame.Maps {
    public class Tile {

        public readonly TileType type;
        public readonly Map Map;
        public readonly Point Position;

        public Tile(TileType type, Map map, Point position) {
            this.type = type;
            this.Map = map;
            this.Position = position;
        }

    }
}