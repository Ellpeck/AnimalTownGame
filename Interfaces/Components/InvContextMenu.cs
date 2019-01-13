using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Interfaces.Components {
    public class InvContextMenu : InterfaceComponent {

        public readonly ItemSlot Slot;
        public readonly RectangleF Bounds;
        private readonly Inventory inventory;

        public InvContextMenu(Inventory iface, ItemSlot slot, Vector2 position) : base(iface) {
            this.Slot = slot;
            this.inventory = iface;
            this.Bounds = new RectangleF(position, new Size2(20, 30));

            var item = slot.Items[slot.Index];
            if (item != null) {
                this.Components.AddRange(item.GetContextMenu(this.Slot, this));
            }
        }

        public void Close() {
            this.inventory.Components.Remove(this);
            this.inventory.ContextMenu = null;
        }

        public override void Draw(SpriteBatch batch) {
            batch.FillRectangle(this.Bounds, new Color(Color.Black, 0.8F));
            base.Draw(batch);
        }

        public override int GetPriority() {
            return 2;
        }

        public override bool IsMouseOver() {
            return this.Bounds.Contains(MousePos);
        }

    }
}