using AnimalTownGame.Main;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Interfaces.Components {
    public class ComponentButton : InterfaceComponent {

        private readonly string text;
        private readonly OnPress callback;
        public RectangleF Bounds;

        public ComponentButton(Interface iface, RectangleF bounds, string text, OnPress callback) : base(iface) {
            this.text = text;
            this.callback = callback;
            this.Bounds = bounds;
        }

        public override void Draw(SpriteBatch batch) {
            var color = this.IsMousedComponent() ? Color.Yellow : Color.White;
            batch.DrawCenteredString(InterfaceManager.NormalFont, this.text,
                this.Bounds.Position + (Vector2) this.Bounds.Size / 2, true, true, color, 0.15F);
            base.Draw(batch);
        }

        public override bool OnMouse(MouseButton button, PressType type) {
            return this.IsMousedComponent() && this.callback(button, type) || base.OnMouse(button, type);
        }

        public override bool IsMouseOver() {
            return this.Bounds.Contains(MousePos);
        }

        public delegate bool OnPress(MouseButton button, PressType type);

    }
}