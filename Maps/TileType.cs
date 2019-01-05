using Microsoft.Xna.Framework;

namespace AnimalTownGame.Maps {
    public class TileType {

        public readonly Point TextureCoord;

        public TileType(Point textureCoord) {
            this.TextureCoord = textureCoord;
        }

        public Tile Instance(Map map, Point position) {
            return new Tile(this, map, position);
        }

    }
}