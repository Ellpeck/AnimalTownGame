using AnimalTownGame.Main;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Interfaces.Components {
    public class ComponentButton : InterfaceComponent {

        private readonly string text;
        private readonly OnPress callback;
        private readonly RectangleF bounds;

        public ComponentButton(Interface iface, RectangleF bounds, string text, OnPress callback) : base(iface) {
            this.text = text;
            this.callback = callback;
            this.bounds = bounds;
        }

        public override void Draw(SpriteBatch batch) {
            var color = this.IsMousedComponent() ? Color.Yellow : Color.White;
            batch.DrawCenteredString(InterfaceManager.NormalFont, this.text,
                this.bounds.Position + (Vector2) this.bounds.Size / 2, true, true, color, 0.15F);
            base.Draw(batch);
        }

        public override bool OnMouse(MouseButton button, PressType type) {
            return this.IsMousedComponent() && this.callback(button, type) || base.OnMouse(button, type);
        }

        public override bool IsMouseOver() {
            return this.bounds.Contains(MousePos);
        }

        public delegate bool OnPress(MouseButton button, PressType type);

    }
}