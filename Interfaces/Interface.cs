using System;
using System.Collections;
using System.Collections.Generic;
using AnimalTownGame.Interfaces.Components;
using AnimalTownGame.Items;
using AnimalTownGame.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace AnimalTownGame.Interfaces {
    public class Interface {

        public static Vector2 MousePos => Mouse.GetState().Position.ToVector2() / InterfaceManager.Scale;
        public readonly List<InterfaceComponent> Components = new List<InterfaceComponent>();

        public void AddComponent(InterfaceComponent comp) {
            this.Components.Add(comp);
            this.Components.Sort();
        }

        public virtual void Update(GameTime time) {
            for (var i = 0; i < this.Components.Count; i++)
                this.Components[i].Update(time);
        }

        public virtual void Draw(SpriteBatch batch) {
            for (var i = this.Components.Count - 1; i >= 0; i--)
                this.Components[i].Draw(batch);
        }

        public virtual void Init(Viewport viewport, Size2 viewportSize) {
            this.Components.Clear();
        }

        public virtual bool OnMouse(MouseButton button, PressType type) {
            for (var i = 0; i < this.Components.Count; i++)
                if (this.Components[i].OnMouse(button, type))
                    return true;
            return false;
        }

        public virtual bool OnKeyboard(string bind, PressType type) {
            for (var i = 0; i < this.Components.Count; i++)
                if (this.Components[i].OnKeyboard(bind, type))
                    return true;
            return false;
        }

        public virtual void OnOpen() {
        }

        public virtual void OnClose() {
        }

        public InterfaceComponent GetMousedComponent() {
            foreach (var comp in this.Components)
                if (comp.IsMouseOver())
                    return comp;
            return null;
        }

    }
}