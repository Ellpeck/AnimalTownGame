using System;
using System.Collections.Generic;
using AnimalTownGame.Interfaces;
using AnimalTownGame.Maps;
using AnimalTownGame.Objects;
using AnimalTownGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace AnimalTownGame.Main {
    public static class InputManager {

        private static readonly Dictionary<string, Keybind> Keybinds = new Dictionary<string, Keybind>();
        private static readonly PressType[] MousePressTypes = new PressType[2];

        static InputManager() {
            AddKeybind(new Keybind("Up", Keys.W));
            AddKeybind(new Keybind("Left", Keys.A));
            AddKeybind(new Keybind("Down", Keys.S));
            AddKeybind(new Keybind("Right", Keys.D));
            AddKeybind(new Keybind("Slow", Keys.LeftShift));
            AddKeybind(new Keybind("Inventory", Keys.E, (oldType, newType) => {
                if (newType == PressType.Pressed)
                    if (InterfaceManager.CurrentInterface == null)
                        InterfaceManager.SetInterface(new Inventory(GameImpl.Instance.Player));
                    else if (InterfaceManager.CurrentInterface is Inventory)
                        InterfaceManager.SetInterface(null);
            }));
            AddKeybind(new Keybind("Escape", Keys.Escape, (oldType, newType) => {
                if (newType == PressType.Pressed && InterfaceManager.CurrentInterface != null)
                    InterfaceManager.SetInterface(null);
            }));
        }

        public static void AddKeybind(Keybind bind) {
            Keybinds.Add(bind.Name, bind);
        }

        public static PressType GetKeyType(string name) {
            return Keybinds[name].Type;
        }

        public static PressType GetMouseType(int index) {
            return MousePressTypes[index];
        }

        public static void Update(Map map, Camera camera) {
            var mouse = Mouse.GetState();
            for (var i = 0; i < 2; i++) {
                var button = i == 0 ? mouse.LeftButton : mouse.RightButton;
                if (button == ButtonState.Pressed) {
                    if (MousePressTypes[i] < PressType.Down)
                        MousePressTypes[i]++;
                } else
                    MousePressTypes[i] = PressType.Nothing;
            }
            if (map != null && InterfaceManager.CurrentInterface == null) {
                var pos = camera.ToWorldPos(mouse.Position.ToVector2());
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