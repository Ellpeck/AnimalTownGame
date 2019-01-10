using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Objects.Characters;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Objects {
    public class Player : Character {

        public Player(Map map, Vector2 position) : base("Player", map, position) {
            this.WalkSpeed = 0.03F;
        }

        public override void Update(GameTime gameTime, bool isCurrent) {
            var speed = InputManager.GetKeyType("Slow") > 0 ? this.WalkSpeed * 0.5F : this.WalkSpeed;
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

            base.Update(gameTime, isCurrent);
        }

        public override bool ShouldTurn() {
            return InputManager.GetKeyType("Slow") == PressType.Nothing;
        }

    }
}