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
        protected Vector2 VelocityDamper = new Vector2(0.5F);

        public DynamicObject(Map map, Vector2 position) : base(map, position) {
        }

        public override void Update(GameTime gameTime, bool isCurrent) {
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
            this.Velocity *= this.VelocityDamper;
        }

        public bool IsCollidingPos(Vector2 pos) {
            return IsCollidingPos(this.Map, pos, this.CollisionBounds, null, this);
        }

        public virtual void Teleport(Map newMap, Vector2 pos) {
            this.Map.Objects.Remove(this);
            this.Map = newMap;
            this.Map.AddObject(this);
            this.Position = pos;
        }

    }
}