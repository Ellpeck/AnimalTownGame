using System;
using System.Collections.Generic;
using AnimalTownGame.Rendering;
using Microsoft.Xna.Framework.Input;

namespace AnimalTownGame.Main {
    public static class InputManager {

        private static readonly Dictionary<string, Keybind> Keybinds = new Dictionary<string, Keybind>();

        static InputManager() {
            AddKeybind(new Keybind("Up", Keys.W));
            AddKeybind(new Keybind("Left", Keys.A));
            AddKeybind(new Keybind("Down", Keys.S));
            AddKeybind(new Keybind("Right", Keys.D));
            AddKeybind(new Keybind("Slow", Keys.LeftShift));
            AddKeybind(new Keybind("Inventory", Keys.E));
            AddKeybind(new Keybind("Escape", Keys.Escape));

            AddKeybind(new Keybind("FrustumCheck", Keys.F1, (oldType, newType) => {
                if (newType == PressType.Pressed)
                    MapRenderer.FrustumTest = MapRenderer.FrustumTest == 1 ? 0 : 1;
            }));
            AddKeybind(new Keybind("CollisionCheck", Keys.F2, (oldType, newType) => {
                if (newType == PressType.Pressed)
                    MapRenderer.DisplayCollisions = !MapRenderer.DisplayCollisions;
            }));
            AddKeybind(new Keybind("RenderCheck", Keys.F3, (oldType, newType) => {
                if (newType == PressType.Pressed)
                    MapRenderer.DisplayRenderBounds = !MapRenderer.DisplayRenderBounds;
            }));
        }

        public static void AddKeybind(Keybind bind) {
            Keybinds.Add(bind.Name, bind);
        }

        public static PressType GetKeyType(string name) {
            return Keybinds[name].Type;
        }

        public static void Update() {
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
}