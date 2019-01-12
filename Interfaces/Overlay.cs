using AnimalTownGame.Main;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Interfaces {
    public class Overlay : Interface {

        private static readonly Texture2D Texture = GameImpl.LoadContent<Texture2D>("Interfaces/Overlay");
        private Vector2 position;
        private Vector2 timeOffset;
        private Vector2 dateOffset;

        public Overlay() {
            var view = GameImpl.Instance.GraphicsDevice.Viewport;
            this.InitPositions(view, new Size2(view.Width, view.Height) / InterfaceManager.Scale);
        }

        public override void Draw(SpriteBatch batch) {
            batch.Draw(Texture, this.position, Color.White);
            batch.DrawCenteredString(InterfaceManager.NormalFont,
                GameImpl.CurrentTime.ToString("h:mm tt"),
                this.position + this.timeOffset, true, true, Color.White, 0.325F);
            batch.DrawCenteredString(InterfaceManager.NormalFont,
                GameImpl.CurrentTime.ToString("dddd d MMM"),
                this.position + this.dateOffset, true, true, Color.White, 0.15F);
        }

        public override void InitPositions(Viewport viewport, Size2 viewportSize) {
            this.position = new Vector2(2, 0);
            this.timeOffset = new Vector2(24, 12);
            this.dateOffset = new Vector2(24, 25.5F);
        }

    }
}