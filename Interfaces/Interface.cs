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
        public Item CursorItem;

        public virtual void Update(GameTime time) {
            for (var i = 0; i < this.Components.Count; i++)
                this.Components[i].Update(time);
        }

        public virtual void Draw(SpriteBatch batch) {
            if (this.Components.Count > 0) {
                var sortedComps = new List<InterfaceComponent>(this.Components);
                sortedComps.Sort();
                foreach (var comp in sortedComps)
                    comp.Draw(batch);
            }

            if (this.CursorItem != null)
                this.CursorItem.Draw(batch, MousePos + Vector2.One, 0.75F);
        }

        public virtual void Init(Viewport viewport, Size2 viewportSize) {
            this.Components.Clear();
        }

        public virtual bool OnMouse(MouseButton button, PressType type) {
            return false;
        }

        public virtual bool OnKeyboard(string bind, PressType type) {
            return false;
        }

        public virtual void OnOpen() {
        }

        public virtual void OnClose() {
            if (this.CursorItem != null)
                foreach (var comp in this.Components) {
                    var slot = comp as ItemSlot;
                    if (slot == null || slot.Items[slot.Index] != null)
                        continue;
                    slot.Items[slot.Index] = this.CursorItem;
                    this.CursorItem = null;
                    break;
                }
        }

    }
}