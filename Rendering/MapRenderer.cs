using System;
using System.Collections.Generic;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using AnimalTownGame.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Rendering {
    public static class MapRenderer {

        public static void RenderMap(SpriteBatch batch, Map map, Viewport viewport, Camera camera) {
            var topLeft = camera.ToWorldPos(Vector2.Zero);
            var bottomRight = camera.ToWorldPos(new Vector2(viewport.Width, viewport.Height));
            var fX = Math.Max(0, topLeft.X.Floor());
            var fY = Math.Max(0, topLeft.Y.Floor());
            var frustum = new RectangleF(fX, fY,
                Math.Min(map.WidthInTiles, (bottomRight.X - fX).Ceil()),
                Math.Min(map.HeightInTiles, (bottomRight.Y - fY).Ceil()));

            batch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointClamp, null, null, null, camera.ViewMatrix);
            for (var x = (int) frustum.X; x < frustum.Right; x++) {
                for (var y = (int) frustum.Y; y < frustum.Bottom; y++) {
                    var tile = map[x, y];
                    if (tile != null)
                        tile.Draw(batch);
                }
            }
            DrawObjects(batch, map.StaticObjects, frustum);
            DrawObjects(batch, map.DynamicObjects, frustum);
            batch.End();
        }

        private static void DrawObjects(SpriteBatch batch, IEnumerable<MapObject> objects, RectangleF frustum) {
            foreach (var obj in objects) {
                if (obj.RenderBounds == RectangleF.Empty)
                    continue;
                var rect = new RectangleF(obj.Position + obj.RenderBounds.Position, obj.RenderBounds.Size);
                if (!frustum.Intersects(rect))
                    continue;
                obj.Draw(batch);
            }
        }

    }
}