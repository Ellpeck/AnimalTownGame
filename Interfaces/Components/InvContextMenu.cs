using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Interfaces.Components {
    public class InvContextMenu : InterfaceComponent {

        public readonly ItemSlot Slot;
        public readonly RectangleF Area;

        public InvContextMenu(Interface iface, ItemSlot slot) : base(iface, slot.Position) {
            this.Slot = slot;
            this.Area = new RectangleF(this.Position, new Size2(40, 60));
        }

        public override void Draw(SpriteBatch batch) {
            batch.FillRectangle(this.Area, new Color(Color.Black, 0.95F));
            base.Draw(batch);
        }

        public override int GetPriority() {
            return 2;
        }

        public override bool IsMouseOver() {
            return this.Area.Contains(MousePos);
        }

    }
}