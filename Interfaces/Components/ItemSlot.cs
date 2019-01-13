using AnimalTownGame.Items;
using AnimalTownGame.Main;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Interfaces.Components {
    public class ItemSlot : InterfaceComponent {

        public readonly Item[] Items;
        public readonly int Index;

        public ItemSlot(Interface iface, Vector2 position, Item[] items, int index) : base(iface, position) {
            this.Items = items;
            this.Index = index;
        }

        public override bool OnMouse(MouseButton button, PressType type) {
            if (!this.Contains(Interface.MousePos))
                return false;
            if (button == MouseButton.Left && type == PressType.Pressed) {
                var temp = this.Interface.CursorItem;
                this.Interface.CursorItem = this.Items[this.Index];
                this.Items[this.Index] = temp;
                return true;
            }
            return false;
        }

        public override void Draw(SpriteBatch batch) {
            var item = this.Items[this.Index];
            if (item != null) {
                item.Draw(batch, this.Position, 1F);

                var mouse = Interface.MousePos;
                if (this.Interface.CursorItem == null && this.Contains(mouse)) {
                    batch.DrawInfoBox(InterfaceManager.NormalFont, item.GetName(), mouse + new Vector2(4, 3), 0.2F);
                }
            }
        }

        public bool Contains(Vector2 pos) {
            return Vector2.DistanceSquared(this.Position + Vector2.One * 7.5F, pos) <= 7.5F * 7.5F;
        }

        public override int GetRenderDepth() {
            return this.Contains(Interface.MousePos) ? 1 : 0;
        }

    }
}