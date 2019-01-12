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
        private static Point lastMousePos;

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
                inter.InitPositions(view, new Size2(view.Width, view.Height) / Scale);
                inter.OnOpen();
            }
        }

        public static void Update(GameTime time) {
            SetCursorType(CursorType.Default, 1F);
            if (CurrentInterface != null)
                CurrentInterface.Update(time);
        }

        public static void Draw(SpriteBatch batch) {
            batch.Begin(
                SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null,
                Matrix.CreateScale(Scale));
            Overlay.Draw(batch);
            if (CurrentInterface != null)
                CurrentInterface.Draw(batch);
            batch.End();

            if (currentCursor >= 0) {
                batch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
                batch.Draw(
                    CursorsTexture.Texture,
                    new Rectangle(lastMousePos, new Point(32, 32)),
                    CursorsTexture.GetRegion((int) currentCursor % 4, (int) currentCursor / 4),
                    Color.Multiply(Color.White, cursorAlpha));
                batch.End();
            }
            lastMousePos = Mouse.GetState().Position;
        }

        public static void OnWindowSizeChange(Viewport viewport) {
            var size = new Size2(viewport.Width, viewport.Height) / Scale;
            Overlay.InitPositions(viewport, size);
            if (CurrentInterface != null)
                CurrentInterface.InitPositions(viewport, size);
        }

    }
}