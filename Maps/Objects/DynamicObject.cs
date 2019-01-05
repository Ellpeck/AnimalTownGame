using System;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AnimalTownGame.Maps.Objects {
    public class DynamicObject : MapObject {

        public Vector2 LastPosition;
        public Vector2 Velocity;
        public Direction Direction = Direction.Down;
        public bool NoClip;

        public DynamicObject(Map map, Vector2 position, Size2 size) : base(map, position, size) {
        }

        public override void Update(TimeSpan passed) {
            base.Update(passed);

            this.LastPosition = this.Position;
            if (this.Velocity.X != 0) {
                var newX = new Vector2(this.Position.X + this.Velocity.X, this.Position.Y);
                if (this.NoClip || !this.IsCollidingPos(newX)) {
                    this.Position = newX;
                }
                if (Math.Abs(this.Velocity.X) >= 0.02) {
                    this.Direction = this.Velocity.X > 0 ? Direction.Right : Direction.Left;
                }
            }
            if (this.Velocity.Y != 0) {
                var newY = new Vector2(this.Position.X, this.Position.Y + this.Velocity.Y);
                if (this.NoClip || !this.IsCollidingPos(newY)) {
                    this.Position = newY;
                }
                if (Math.Abs(this.Velocity.Y) >= 0.02) {
                    this.Direction = this.Velocity.Y > 0 ? Direction.Down : Direction.Up;
                }
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