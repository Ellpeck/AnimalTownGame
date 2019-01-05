using System;
using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimalTownGame.Rendering {
    public static class MapRenderer {

        public static void RenderMap(SpriteBatch batch, Map map, Viewport viewport, Camera camera) {
            var topLeft = camera.ToWorldPos(Vector2.Zero);
            if (topLeft.X < 0)
                topLeft.X = 0;
            if (topLeft.Y < 0)
                topLeft.Y = 0;
            var bottomRight = camera.ToWorldPos(new Vector2(viewport.Width, viewport.Height));
            if (bottomRight.X > map.WidthInTiles)
                bottomRight.X = map.WidthInTiles;
            if (bottomRight.Y > map.HeightInTiles)
                bottomRight.Y = map.HeightInTiles;

            var drawn = 0;
            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.ViewMatrix);
            var atlas = Registry.TextureOutside;
            for (var x = (int) topLeft.X; x < bottomRight.X; x++) {
                for (var y = (int) topLeft.Y; y < bottomRight.Y; y++) {
                    var tile = map[x, y];
                    if (tile == null)
                        continue;
                    var texCoord = tile.type.TextureCoord;
                    batch.Draw(
                        atlas.Texture,
                        new Rectangle(tile.Position.X, tile.Position.Y, 1, 1),
                        atlas.GetRegion(texCoord.X, texCoord.Y),
                        Color.White);
                    drawn++;
                }
            }
            batch.End();
            Console.WriteLine(drawn);
        }

    }
}