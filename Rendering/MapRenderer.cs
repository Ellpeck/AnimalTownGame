using System;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using AnimalTownGame.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Rendering {
    public static class MapRenderer {

        public static int FrustumTest;
        public static bool DisplayCollisions;
        public static bool DisplayRenderBounds;

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
                    if (tile == null)
                        continue;
                    tile.Draw(batch);

                    if (DisplayCollisions) {
                        var bounds = tile.GetCollisionBounds();
                        if (bounds != Rectangle.Empty)
                            batch.DrawRectangle(bounds.Location.ToVector2(), bounds.Size, Color.Red, 1F / camera.Scale);
                    }
                }
            }
            batch.End();

            batch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, null, null, null, camera.ViewMatrix);
            foreach (var obj in map.DynamicObjects)
                DrawObj(batch, obj, camera, frustum);
            foreach (var obj in map.StaticObjects)
                DrawObj(batch, obj, camera, frustum);
            batch.End();
        }

        private static void DrawObj(SpriteBatch batch, MapObject obj, Camera camera, RectangleF frustum) {
            if (obj.RenderBounds == Rectangle.Empty)
                return;
            var rect = new RectangleF(obj.Position + obj.RenderBounds.Position, obj.RenderBounds.Size);
            if (!frustum.Intersects(rect))
                return;

            obj.Draw(batch);

            if (DisplayCollisions) {
                var bounds = obj.CollisionBounds;
                bounds.Offset(obj.Position);
                batch.DrawRectangle(bounds.Position, bounds.Size, Color.Blue, 1F / camera.Scale);
            }
            if (DisplayRenderBounds) {
                var bounds = obj.RenderBounds;
                bounds.Offset(obj.Position);
                batch.DrawRectangle(bounds.Position, bounds.Size, Color.Purple, 1F / camera.Scale);
            }
        }

    }
}