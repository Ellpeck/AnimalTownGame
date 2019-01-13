using System;
using System.Collections.Generic;
using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Objects {
    public class MapObject {

        public Map Map;
        public RectangleF RenderBounds;
        public RectangleF CollisionBounds;
        public RectangleF HighlightBounds;
        public float DepthOffset;
        public Vector2 Position;

        public MapObject(Map map, Vector2 position) {
            this.Map = map;
            this.Position = position;
        }

        public virtual void UpdateRealTime(DateTime now, DateTime lastUpdate, TimeSpan passed) {
        }

        public virtual void Draw(SpriteBatch batch) {
        }

        public float GetRenderDepth() {
            return Math.Max(0, (this.Position.Y + this.DepthOffset) / this.Map.HeightInTiles / 1000F);
        }

        public virtual bool OnMouse(Vector2 pos, MouseButton button, PressType type) {
            return false;
        }

        public static bool IsCollidingPos(Map map, Vector2 pos, RectangleF collisionBounds, MapObject thisObj) {
            if (!map.IsInBounds(pos.X.Floor(), pos.Y.Floor()))
                return true;
            if (collisionBounds == RectangleF.Empty)
                return false;
            collisionBounds.Offset(pos);
            for (var x = 0; x <= collisionBounds.Width.Ceil(); x++)
                for (var y = 0; y <= collisionBounds.Height.Ceil(); y++) {
                    var tile = map[collisionBounds.X.Floor() + x, collisionBounds.Y.Floor() + y];
                    if (tile == null)
                        continue;
                    var bounds = tile.GetCollisionBounds();
                    if (bounds != Rectangle.Empty && collisionBounds.Intersects(bounds))
                        return true;
                }

            if (Collides(collisionBounds, map.StaticObjects, thisObj))
                return true;
            if (Collides(collisionBounds, map.DynamicObjects, thisObj))
                return true;

            return false;
        }

        private static bool Collides(RectangleF myBounds, IEnumerable<MapObject> objects, MapObject thisObj) {
            foreach (var obj in objects) {
                if (obj == thisObj)
                    continue;
                var otherBounds = obj.CollisionBounds;
                if (otherBounds == RectangleF.Empty)
                    continue;
                otherBounds.Offset(obj.Position);
                if (thisObj != null) {
                    var curr = thisObj.CollisionBounds;
                    curr.Offset(thisObj.Position);
                    if (curr.Intersects(otherBounds))
                        continue;
                }
                if (myBounds.Intersects(otherBounds))
                    return true;
            }
            return false;
        }

    }
}