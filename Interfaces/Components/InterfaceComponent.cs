using System;
using AnimalTownGame.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimalTownGame.Interfaces.Components {
    public class InterfaceComponent : IComparable<InterfaceComponent> {

        public readonly Interface Interface;
        public readonly Vector2 Position;

        public InterfaceComponent(Interface iface, Vector2 position) {
            this.Interface = iface;
            this.Position = position;
        }

        public virtual void Update(GameTime time) {
        }

        public virtual void Draw(SpriteBatch batch) {
        }

        public virtual bool OnMouse(MouseButton button, PressType type) {
            return false;
        }

        public virtual bool OnKeyboard(string bind, PressType type) {
            return false;
        }

        public virtual int GetRenderDepth() {
            return 0;
        }

        public int CompareTo(InterfaceComponent other) {
            return this.GetRenderDepth().CompareTo(other.GetRenderDepth());
        }

    }
}