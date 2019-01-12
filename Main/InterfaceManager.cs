using AnimalTownGame.Interfaces;
using AnimalTownGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AnimalTownGame.Main {
    public static class InterfaceManager {

        private static readonly TextureAtlas CursorsTexture = new TextureAtlas("Interfaces/Cursors", 16, 16);
        public static SpriteFont NormalFont = GameImpl.LoadContent<SpriteFont>("Interfaces/NormalFont");

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
                inter.InitPositions(GameImpl.Instance.GraphicsDevice.Viewport);
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

        public static bool HandleMouse(Point pos, PressType[] pressTypes) {
            if (CurrentInterface == null)
                return false;
            if (CurrentInterface.OnMouse(pos, pressTypes))
                return true;
            foreach (var component in CurrentInterface.Components)
                if (component.OnMouse(pos, pressTypes))
                    return true;
            return true;
        }

        public static bool HandleKeyboard(string bind, PressType type) {
            if (CurrentInterface == null)
                return false;
            if (CurrentInterface.OnKeyboard(bind, type))
                return true;
            foreach (var component in CurrentInterface.Components)
                if (component.OnKeyboard(bind, type))
                    return true;
            return true;
        }

        public static void OnWindowSizeChange(Viewport viewport) {
            Overlay.InitPositions(viewport);
            if (CurrentInterface != null)
                CurrentInterface.InitPositions(viewport);
        }

    }
}