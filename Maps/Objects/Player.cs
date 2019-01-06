using System;
using AnimalTownGame.Main;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Maps.Objects {
    public class Player : Character {

        public Player(Map map, Vector2 position) : base("Player", map, position) {
        }

        public override void Update(GameTime gameTime) {
            var speed = InputManager.GetKeyType("Slow") > 0 ? 0.02F : 0.03F;
            var vel = new Vector2();

            if (InputManager.GetKeyType("Up") > 0) {
                vel.Y -= speed;
            }
            if (InputManager.GetKeyType("Down") > 0) {
                vel.Y += speed;
            }

            if (InputManager.GetKeyType("Left") > 0) {
                vel.X -= speed;
            }
            if (InputManager.GetKeyType("Right") > 0) {
                vel.X += speed;
            }

            if (vel.X != 0 && vel.Y != 0) {
                vel /= 1.42F;
            }
            this.Velocity += vel;

            base.Update(gameTime);
        }

    }
}