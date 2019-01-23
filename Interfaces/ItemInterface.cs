using System.Collections.Generic;
using AnimalTownGame.Interfaces.Components;
using AnimalTownGame.Items;
using AnimalTownGame.Objects.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimalTownGame.Interfaces {
    public class ItemInterface : Interface {

        public readonly List<ItemSlot> Slots = new List<ItemSlot>();
        public Item CursorItem;
        public readonly Player Player;

        public ItemInterface(Player player) {
            this.Player = player;
        }

        public void AddSlot(ItemSlot slot) {
            this.AddComponent(slot);
            this.Slots.Add(slot);
        }

        public override void Draw(SpriteBatch batch) {
            base.Draw(batch);
            if (this.CursorItem != null)
                this.CursorItem.Draw(batch, MousePos + Vector2.One * 1.5F, 0.75F, 0, true);
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

        public void AddSlotGrid(Vector2 position, int width, int height, Item[] slots) {
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    this.AddSlot(new ItemSlot(this,
                        position + new Vector2(x * 16 + (y % 2 == 0 ? 0 : 8), y * 14),
                        slots, width * y + x));
        }

    }
}