using System;
using System.Collections.Generic;
using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using AnimalTownGame.Objects.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Objects {
    public class MapObject {

        public Map Map;
        public RectangleF RenderBounds;
        public RectangleF CollisionBounds;
        public RectangleF HighlightBounds;
        public RectangleF IntersectionBounds;
        public float DepthOffset;
        public Vector2 Position;

        public MapObject(Map map, Vector2 position) {
            this.Map = map;
            this.Position = position;
        }

        public virtual void Update(GameTime gameTime, bool isCurrent) {
        }

        public virtual void UpdateRealTime(DateTime now, DateTime lastUpdate, TimeSpan passed) {
        }

        public virtual void Draw(SpriteBatch batch) {
        }

        public virtual void OnIntersectWith(Character obj) {
        }

        public float GetRenderDepth() {
            return Math.Max(0, (this.Position.Y + this.DepthOffset) / this.Map.HeightInTiles / 1000F);
        }

        public virtual bool OnMouse(Vector2 pos, MouseButton button, PressType type) {
            return false;
        }

        public static bool IsCollidingPos(Map map, Vector2 pos, RectangleF bounds, Map.ObjectSelector objSelector = null, MapObject thisObj = null) {
            if (!map.IsInBounds(pos.X.Floor(), pos.Y.Floor()))
                return true;
            if (bounds == RectangleF.Empty)
                return false;
            bounds.Offset(pos);

            foreach (var tile in map.GetTilesInArea(bounds)) {
                if (bounds.IntersectsNonEmpty(tile.GetCollisionBounds()))
                    return true;
            }

            if (objSelector == null && thisObj != null)
                objSelector = obj => obj != thisObj;
            foreach (var obj in map.GetObjectsInArea(bounds, obj => obj.CollisionBounds, objSelector)) {
                if (thisObj != null) {
                    var other = obj.CollisionBounds.Move(obj.Position);
                    var curr = thisObj.CollisionBounds.Move(thisObj.Position);
                    if (curr.IntersectsNonEmpty(other))
                        continue;
                }
                return true;
            }

            return false;
        }

    }
}