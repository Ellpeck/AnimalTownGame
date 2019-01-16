using System;
using System.Collections.Generic;
using AnimalTownGame.Interfaces;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
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
                if (newType == PressType.Pressed) {
                    if (InterfaceManager.CurrentInterface == null)
                        InterfaceManager.SetInterface(new Inventory(GameImpl.Instance.Player));
                    else if (InterfaceManager.CurrentInterface is Inventory)
                        InterfaceManager.SetInterface(null);
                    return true;
                }
                return false;
            }));
            AddKeybind(new Keybind("Escape", Keys.Escape, (oldType, newType) => {
                if (newType == PressType.Pressed && InterfaceManager.CurrentInterface != null) {
                    InterfaceManager.SetInterface(null);
                    return true;
                }
                return false;
            }));
            AddKeybind(new Keybind("CheatItems", Keys.F1, (oldType, newType) => {
                if (newType == PressType.Pressed) {
                    var i = 0;
                    foreach (var type in Registry.ItemTypes.Values)
                        GameImpl.Instance.Player.Inventory[i++] = type.Instance();
                    return true;
                }
                return false;
            }));
            AddKeybind(new Keybind("Debug", Keys.F2, (oldType, newType) => {
                if (newType == PressType.Pressed) {
                    MapRenderer.DisplayBounds = !MapRenderer.DisplayBounds;
                    return true;
                }
                return false;
            }));
        }

        public static void AddKeybind(Keybind bind) {
            Keybinds.Add(bind.Name, bind);
        }

        public static PressType GetKeyType(string name) {
            return Keybinds[name].Type;
        }

        public static PressType GetMouseType(MouseButton button) {
            return MousePressTypes[(int) button];
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

                var mouseButton = (MouseButton) i;
                var type = MousePressTypes[i];
                if (!InterfaceManager.HandleMouse(mouseButton, type)) {
                    if (map != null && InterfaceManager.CurrentInterface == null) {
                        var pos = camera.ToWorldPos(mouse.Position.ToVector2());
                        foreach (var obj in map.AllObjects) {
                            var bounds = obj.HighlightBounds.Move(obj.Position);
                            if (bounds != RectangleF.Empty && bounds.Contains(pos))
                                if (obj.OnMouse(pos, mouseButton, type))
                                    break;
                        }
                    }
                }
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
                    var old = bind.Type;
                    bind.Type = newType;
                    if (bind.OnChange != null)
                        if (bind.OnChange(old, newType))
                            continue;
                }

                InterfaceManager.HandleKeyboard(bind.Name, newType);
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

        public delegate bool OnTypeChange(PressType oldType, PressType newType);

    }

    public enum PressType {

        Nothing,
        Pressed,
        Down

    }

    public enum MouseButton {

        Left,
        Right

    }
}