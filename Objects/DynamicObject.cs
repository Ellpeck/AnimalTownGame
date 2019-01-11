using System;
using System.Collections.Generic;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AnimalTownGame.Objects {
    public class DynamicObject : MapObject {

        public Vector2 LastPosition;
        public Vector2 Velocity;
        public bool NoClip;

        public DynamicObject(Map map, Vector2 position) : base(map, position) {
        }

        public virtual void Update(GameTime gameTime, bool isCurrent) {
            this.LastPosition = this.Position;
            if (this.Velocity.X != 0) {
                var newX = new Vector2(this.Position.X + this.Velocity.X, this.Position.Y);
                if (this.NoClip || !this.IsCollidingPos(newX))
                    this.Position = newX;
            }
            if (this.Velocity.Y != 0) {
                var newY = new Vector2(this.Position.X, this.Position.Y + this.Velocity.Y);
                if (this.NoClip || !this.IsCollidingPos(newY))
                    this.Position = newY;
            }
            this.Velocity *= 0.5F;
        }

        public bool IsCollidingPos(Vector2 pos) {
            if (!this.Map.IsInBounds(pos.X.Floor(), pos.Y.Floor()))
                return true;

            var myBounds = this.CollisionBounds;
            myBounds.Offset(pos);
            for (var x = 0; x <= myBounds.Width.Ceil(); x++)
                for (var y = 0; y <= myBounds.Height.Ceil(); y++) {
                    var tile = this.Map[myBounds.X.Floor() + x, myBounds.Y.Floor() + y];
                    if (tile == null)
                        continue;
                    var bounds = tile.GetCollisionBounds();
                    if (bounds != Rectangle.Empty && myBounds.Intersects(bounds))
                        return true;
                }

            if (this.Collides(myBounds, this.Map.StaticObjects))
                return true;
            if (this.Collides(myBounds, this.Map.DynamicObjects))
                return true;

            return false;
        }

        private bool Collides(RectangleF myBounds, IEnumerable<MapObject> objects) {
            foreach (var obj in objects) {
                if (obj == this)
                    continue;
                var otherBounds = obj.CollisionBounds;
                otherBounds.Offset(obj.Position);
                if (myBounds.Intersects(otherBounds))
                    return true;
            }
            return false;
        }

        public virtual void Teleport(Map newMap, Vector2 pos) {
            this.Map.DynamicObjects.Remove(this);
            this.Map = newMap;
            this.Map.DynamicObjects.Add(this);
            this.Position = pos;
        }

    }
}