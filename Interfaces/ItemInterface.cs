using System.Collections.Generic;
using AnimalTownGame.Interfaces.Components;
using AnimalTownGame.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimalTownGame.Interfaces {
    public class ItemInterface : Interface {

        public readonly List<ItemSlot> Slots = new List<ItemSlot>();
        public Item CursorItem;

        public void AddSlot(ItemSlot slot) {
            this.AddComponent(slot);
            this.Slots.Add(slot);
        }

        public override void Draw(SpriteBatch batch) {
            base.Draw(batch);
            if (this.CursorItem != null)
                this.CursorItem.Draw(batch, MousePos + Vector2.One, 0.75F);
        }

        public override void OnClose() {
            if (this.CursorItem != null)
                foreach (var slot in this.Slots) {
                    if (slot.Items[slot.Index] != null)
                        continue;
                    slot.Items[slot.Index] = this.CursorItem;
                    this.CursorItem = null;
                    break;
                }
        }

    }
}