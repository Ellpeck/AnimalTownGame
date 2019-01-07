using System;
using AnimalTownGame.Main;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Maps {
    public static class MapGenerator {

        public static Map GenerateTown() {
            var rand = new Random();
            var map = new Map("Town", 64, 64);

            // Initial grass
            for (var x = 0; x < map.WidthInTiles; x++)
                for (var y = 0; y < map.HeightInTiles; y++)
                    map.SetTile(new Point(x, y), rand.NextDouble() >= 0.6 ? Registry.TileDarkGrass : Registry.TileGrass);

            return map;
        }

    }
}