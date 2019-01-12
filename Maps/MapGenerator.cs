using System;
using AnimalTownGame.Main;
using AnimalTownGame.Misc;
using AnimalTownGame.Objects.Static;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Maps {
    public static class MapGenerator {

        public static Map GenerateTown() {
            var rand = new Random();
            var map = new Map("Town", 64, 64, false);

            // Initial grass
            for (var x = 0; x < map.WidthInTiles; x++)
                for (var y = 0; y < map.HeightInTiles; y++)
                    map.SetTile(new Point(x, y), rand.NextDouble() >= 0.6 ? Registry.TileDarkGrass : Registry.TileGrass);

            return map;
        }

        public static Map GenerateHouse(string name, Point townPosition) {
            var map = new Map(name, 11, 13, true, new Vector2(5.5F, 11.5F));

            for (var x = 0; x < map.WidthInTiles; x++)
                for (var y = 0; y < map.HeightInTiles; y++) {
                    var tile = Registry.TileFloor;
                    if ((x == 0 || y == 0 || x == map.WidthInTiles - 1 || y >= map.HeightInTiles - 2) && (x != 5 || y != 11))
                        tile = Registry.TileWallTrim;
                    else if (y >= 1 && y <= 3)
                        tile = Registry.TileWall;
                    map.SetTile(new Point(x, y), tile);
                }

            map.StaticObjects.Add(new Teleporter(map, new Vector2(5, 12.75F), "Town", townPosition.ToVector2() + new Vector2(2.5F, 1.15F), Direction.Down));
            return map;
        }

    }
}