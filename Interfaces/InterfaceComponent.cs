using AnimalTownGame.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Interfaces {
    public class InterfaceComponent {

        public readonly Vector2 Position;
        public readonly Size2 Size;

        public InterfaceComponent(Vector2 position, Size2 size) {
            this.Position = position;
            this.Size = size;
        }

        public virtual void Update(GameTime time) {
        }

        public virtual void Draw(SpriteBatch batch) {
        }

        public virtual bool OnMouse(Point pos, PressType[] pressTypes) {
            return false;
        }

        public virtual bool OnKeyboard(string bind, PressType type) {
            return false;
        }
    }
}