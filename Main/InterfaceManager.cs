using AnimalTownGame.Interfaces;
using AnimalTownGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace AnimalTownGame.Main {
    public static class InterfaceManager {

        private static readonly TextureAtlas CursorsTexture = new TextureAtlas("Interfaces/Cursors", 16, 16);
        public static readonly SpriteFont NormalFont = GameImpl.LoadContent<SpriteFont>("Interfaces/NormalFont");

        public const int Scale = 5;
        public static readonly Interface Overlay = new Overlay();
        public static Interface CurrentInterface { get; private set; }
        private static CursorType currentCursor;
        private static float cursorAlpha;

        public static void SetCursorType(CursorType type, float alpha) {
            currentCursor = type;
            cursorAlpha = alpha;
        }

        public static void SetInterface(Interface inter) {
            if (CurrentInterface != null)
                CurrentInterface.OnClose();
            CurrentInterface = inter;
            if (inter != null) {
                var view = GameImpl.Instance.GraphicsDevice.Viewport;
                inter.Init(view, new Size2(view.Width, view.Height) / Scale);
                inter.OnOpen();
            }
        }

        public static void Update(GameTime time) {
            SetCursorType(CursorType.Default, 1F);
            if (CurrentInterface != null)
                CurrentInterface.Update(time);
        }

        public static void HandleMouse(MouseButton button, PressType type) {
            if (CurrentInterface == null)
                return;
            if (CurrentInterface.OnMouse(button, type))
                return;
            foreach (var component in CurrentInterface.Components)
                if (component.OnMouse(button, type))
                    return;
        }

        public static void HandleKeyboard(string bind, PressType type) {
            if (CurrentInterface == null)
                return;
            if (CurrentInterface.OnKeyboard(bind, type))
                return;
            foreach (var component in CurrentInterface.Components)
                if (component.OnKeyboard(bind, type))
                    return;
        }

        public static void Draw(SpriteBatch batch, Viewport viewport) {
            batch.Begin(
                SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null,
                Matrix.CreateScale(Scale));
            Overlay.Draw(batch);
            if (CurrentInterface != null) {
                batch.FillRectangle(Vector2.Zero, new Size2(viewport.Width, viewport.Height), new Color(Color.Black, 0.5F));
                CurrentInterface.Draw(batch);
            }
            batch.End();

            if (currentCursor >= 0) {
                batch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
                batch.Draw(
                    CursorsTexture.Texture,
                    new Rectangle(Mouse.GetState().Position, new Point(32, 32)),
                    CursorsTexture.GetRegion((int) currentCursor % 4, (int) currentCursor / 4),
                    Color.Multiply(Color.White, cursorAlpha));
                batch.End();
            }
        }

        public static void OnWindowSizeChange(Viewport viewport) {
            var size = new Size2(viewport.Width, viewport.Height) / Scale;
            Overlay.Init(viewport, size);
            if (CurrentInterface != null)
                CurrentInterface.Init(viewport, size);
        }
    }

    public enum CursorType {

        Default,
        Door,
        Dialog

    }
}