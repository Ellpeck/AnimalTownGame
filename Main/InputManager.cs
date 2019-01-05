using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace AnimalTownGame.Main {
    public static class InputManager {

        private static readonly Dictionary<string, Keybind> Keybinds = new Dictionary<string, Keybind>();

        static InputManager() {
            AddKeybind("Up", Keys.W);
            AddKeybind("Left", Keys.A);
            AddKeybind("Down", Keys.S);
            AddKeybind("Right", Keys.D);
            AddKeybind("Slow", Keys.LeftShift);
            AddKeybind("Inventory", Keys.E);
            AddKeybind("Escape", Keys.Escape);
        }

        public static void AddKeybind(string name, Keys value) {
            Keybinds.Add(name, new Keybind(name, value));
        }

        public static PressType GetKeyType(string name) {
            return Keybinds[name].Type;
        }

        public static void Update() {
            var keyboard = Keyboard.GetState();
            foreach (var bind in Keybinds.Values) {
                if (keyboard.IsKeyDown(bind.Key)) {
                    if (bind.Type < PressType.Down)
                        bind.Type++;
                } else
                    bind.Type = PressType.Nothing;
            }
        }

    }

    public class Keybind {

        public readonly string Name;
        public Keys Key;
        public PressType Type;

        public Keybind(string name, Keys key) {
            this.Name = name;
            this.Key = key;
        }

    }

    public enum PressType {

        Nothing,
        Pressed,
        Down

    }
}