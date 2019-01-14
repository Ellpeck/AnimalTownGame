using System;
using System.Linq;
using AnimalTownGame.Items;
using AnimalTownGame.Main;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Interfaces.Components {
    public class ItemSlot : InterfaceComponent {

        public readonly Vector2 Position;
        public readonly Item[] Items;
        public readonly int Index;
        public new readonly ItemInterface Interface;
        private ComponentHover hoverInfo;

        public ItemSlot(ItemInterface iface, Vector2 position, Item[] items, int index) : base(iface) {
            this.Items = items;
            this.Index = index;
            this.Interface = iface;
            this.Position = position;
        }

        public override bool OnMouse(MouseButton button, PressType type) {
            if (button == MouseButton.Left && type == PressType.Pressed) {
                if (!this.IsMousedComponent())
                    return false;
                var temp = this.Interface.CursorItem;
                this.Interface.CursorItem = this.Items[this.Index];
                this.Items[this.Index] = temp;
                return true;
            }
            return false;
        }

        public override void Update(GameTime time) {
            base.Update(time);

            var item = this.Items[this.Index];
            if (item != null && this.Interface.CursorItem == null && this.IsMousedComponent()) {
                if (this.hoverInfo == null) {
                    this.hoverInfo = new ComponentHover(this, item.GetDisplayName());
                    this.Interface.AddComponent(this.hoverInfo);
                }
            } else if (this.hoverInfo != null) {
                this.Interface.Components.Remove(this.hoverInfo);
                this.hoverInfo = null;
            }
        }

        public override void Draw(SpriteBatch batch) {
            var item = this.Items[this.Index];
            if (item != null)
                item.Draw(batch, this.Position, 1F);
            base.Draw(batch);
        }

        public override bool IsMouseOver() {
            return Vector2.DistanceSquared(this.Position + Vector2.One * 7.5F, MousePos) <= 7.5F * 7.5F;
        }

    }
}