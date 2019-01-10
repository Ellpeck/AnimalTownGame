using System;
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
                        return true;
                    var bounds = tile.GetCollisionBounds();
                    if (bounds != Rectangle.Empty && myBounds.Intersects(bounds))
                        return true;
                }

            foreach (var obj in this.Map.DynamicObjects)
                if (obj != this && Collides(myBounds, obj))
                    return true;
            foreach (var obj in this.Map.StaticObjects)
                if (Collides(myBounds, obj))
                    return true;

            return false;
        }

        private static bool Collides(RectangleF myBounds, MapObject otherObject) {
            var otherBounds = otherObject.CollisionBounds;
            otherBounds.Offset(otherObject.Position);
            return myBounds.Intersects(otherBounds);
        }

    }
}