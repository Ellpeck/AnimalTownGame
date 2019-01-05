using System;
using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Rendering {
    public static class MapRenderer {

        public static void RenderMap(SpriteBatch batch, Map map, Viewport viewport, Camera camera) {
            var topLeft = camera.ToWorldPos(Vector2.Zero);
            var bottomRight = camera.ToWorldPos(new Vector2(viewport.Width, viewport.Height));
            var fX = Math.Max(0, Util.Floor(topLeft.X));
            var fY = Math.Max(0, Util.Floor(topLeft.Y));
            var frustum = new Rectangle(fX, fY,
                Math.Min(map.WidthInTiles, Util.Ceil(bottomRight.X - fX)),
                Math.Min(map.HeightInTiles, Util.Ceil(bottomRight.Y - fY)));

            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.ViewMatrix);
            var atlas = Registry.TextureOutside;
            for (var x = frustum.X; x < frustum.Right; x++) {
                for (var y = frustum.Y; y < frustum.Bottom; y++) {
                    var tile = map[x, y];
                    if (tile == null)
                        continue;
                    var texCoord = tile.type.TextureCoord;
                    batch.Draw(
                        atlas.Texture,
                        new Rectangle(tile.Position.X, tile.Position.Y, 1, 1),
                        atlas.GetRegion(texCoord.X, texCoord.Y),
                        Color.White);
                }
            }
            batch.End();

            batch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, null, null, null, camera.ViewMatrix);
            foreach (var obj in map.DynamicObjects)
                obj.Draw(batch);
            batch.End();
        }

    }
}