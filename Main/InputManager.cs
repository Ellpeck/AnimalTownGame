using System;
using System.Collections.Generic;
using AnimalTownGame.Maps;
using AnimalTownGame.Objects;
using AnimalTownGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace AnimalTownGame.Main {
    public static class InputManager {

        private static readonly Texture2D CursorsTexture = GameImpl.LoadContent<Texture2D>("Interfaces/Cursors");
        private static readonly Dictionary<string, Keybind> Keybinds = new Dictionary<string, Keybind>();
        private static PressType leftClickPressType;
        private static PressType rightClickPressType;
        private static CursorType currentCursor;
        private static float cursorAlpha;
        private static Point lastMousePos;

        static InputManager() {
            AddKeybind(new Keybind("Up", Keys.W));
            AddKeybind(new Keybind("Left", Keys.A));
            AddKeybind(new Keybind("Down", Keys.S));
            AddKeybind(new Keybind("Right", Keys.D));
            AddKeybind(new Keybind("Slow", Keys.LeftShift));
            AddKeybind(new Keybind("Inventory", Keys.E));
            AddKeybind(new Keybind("Escape", Keys.Escape));
        }

        public static void AddKeybind(Keybind bind) {
            Keybinds.Add(bind.Name, bind);
        }

        public static void SetCursorType(CursorType type, float alpha) {
            currentCursor = type;
            cursorAlpha = alpha;
        }

        public static PressType GetKeyType(string name) {
            return Keybinds[name].Type;
        }

        public static PressType GetLeftMouse() {
            return leftClickPressType;
        }

        public static PressType GetRightMouse() {
            return rightClickPressType;
        }

        public static void Update(Map map, Camera camera) {
            SetCursorType(CursorType.Default, 1F);
            var mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed) {
                if (leftClickPressType < PressType.Down)
                    leftClickPressType++;
            } else
                leftClickPressType = PressType.Nothing;
            if (mouse.RightButton == ButtonState.Pressed) {
                if (rightClickPressType < PressType.Down)
                    rightClickPressType++;
            } else
                rightClickPressType = PressType.Nothing;
            if (map != null) {
                var pos = camera.ToWorldPos(lastMousePos.ToVector2());
                MouseOverObjects(map.StaticObjects, pos);
                MouseOverObjects(map.DynamicObjects, pos);
            }

            var keyboard = Keyboard.GetState();
            foreach (var bind in Keybinds.Values) {
                var newType = bind.Type;
                if (keyboard.IsKeyDown(bind.Key)) {
                    if (bind.Type < PressType.Down)
                        newType = bind.Type + 1;
                } else
                    newType = PressType.Nothing;

                if (newType != bind.Type) {
                    if (bind.OnChange != null)
                        bind.OnChange(bind.Type, newType);
                    bind.Type = newType;
                }
            }
        }

        public static void Draw(SpriteBatch batch) {
            if (currentCursor >= 0) {
                batch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
                batch.Draw(
                    CursorsTexture,
                    new Rectangle(lastMousePos, new Point(32, 32)),
                    new Rectangle((int) currentCursor % 4 * 16, (int) currentCursor / 4 * 16, 16, 16),
                    Color.Multiply(Color.White, cursorAlpha));
                batch.End();
            }
            lastMousePos = Mouse.GetState().Position;
        }

        private static void MouseOverObjects(IEnumerable<MapObject> objects, Vector2 mousePos) {
            foreach (var obj in objects) {
                var bounds = obj.HighlightBounds;
                if (bounds != RectangleF.Empty) {
                    bounds.Offset(obj.Position);
                    if (bounds.Contains(mousePos))
                        obj.OnMouseOver(mousePos);
                }
            }
        }

    }

    public class Keybind {

        public readonly string Name;
        public readonly OnTypeChange OnChange;
        public readonly Keys Key;
        public PressType Type;

        public Keybind(string name, Keys key, OnTypeChange onChange = null) {
            this.Name = name;
            this.Key = key;
            this.OnChange = onChange;
        }

        public delegate void OnTypeChange(PressType oldType, PressType newType);

    }

    public enum PressType {

        Nothing,
        Pressed,
        Down

    }

    public enum CursorType {

        Default,
        Door,
        Dialog

    }
}