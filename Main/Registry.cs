using AnimalTownGame.Maps;
using AnimalTownGame.Rendering;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Main {
    public class Registry {

        public static readonly TextureAtlas TextureOutside = new TextureAtlas("Tilesets/Outside", 16, 16);

        public static readonly TileType TileGrass = new TileType(new Point(4, 1));
        public static readonly TileType TilePath = new TileType(new Point(2, 0));

    }
}