using System;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Objects {
    public class DynamicObject : MapObject {

        public Vector2 LastPosition;
        public Vector2 Velocity;
        public bool NoClip;

        public DynamicObject(Map map, Vector2 position) : base(map, position) {
        }

        public virtual void Update(GameTime gameTime) {
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
            if (pos.X < 0 || pos.Y < 0)
                return true;
            if (pos.X > this.Map.WidthInTiles || pos.Y > this.Map.HeightInTiles)
                return true;
            return false;
        }

    }
}