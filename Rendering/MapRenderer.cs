using System;
using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using AnimalTownGame.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Rendering {
    public static class MapRenderer {

        private const int FrustumTest = 0;

        public static void RenderMap(SpriteBatch batch, Map map, Viewport viewport, Camera camera) {
            var topLeft = camera.ToWorldPos(Vector2.Zero);
            var bottomRight = camera.ToWorldPos(new Vector2(viewport.Width, viewport.Height));
            var fX = Math.Max(0, (topLeft.X + FrustumTest).Floor());
            var fY = Math.Max(0, (topLeft.Y + FrustumTest).Floor());
            var frustum = new RectangleF(fX, fY,
                Math.Min(map.WidthInTiles, (bottomRight.X - FrustumTest - fX).Ceil()),
                Math.Min(map.HeightInTiles, (bottomRight.Y - FrustumTest - fY).Ceil()));

            batch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.ViewMatrix);

            for (var x = (int) frustum.X; x < frustum.Right; x++) {
                for (var y = (int) frustum.Y; y < frustum.Bottom; y++) {
                    var tile = map[x, y];
                    if (tile != null)
                        tile.Draw(batch);
                }
            }
            batch.End();

            batch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, null, null, null, camera.ViewMatrix);
            foreach (var obj in map.DynamicObjects)
                if (OnScreen(obj, frustum))
                    obj.Draw(batch);
            foreach (var obj in map.StaticObjects)
                if (OnScreen(obj, frustum))
                    obj.Draw(batch);
            batch.End();
        }

        private static bool OnScreen(MapObject obj, RectangleF frustum) {
            if (obj.RenderBounds == Rectangle.Empty)
                return false;
            var rect = new RectangleF(obj.Position + obj.RenderBounds.Position, obj.RenderBounds.Size);
            return frustum.Intersects(rect);
        }

    }
}