using System.Collections.Generic;
using AnimalTownGame.Interfaces.Components;
using AnimalTownGame.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Items {
    public class ItemFurniture : Item {

        public new readonly FurnitureType Type;

        public ItemFurniture(FurnitureType type) : base(type) {
            this.Type = type;
        }

        public override IEnumerable<ComponentButton> GetContextMenu(ItemSlot slot, InvContextMenu menu) {
            var map = GameImpl.Instance.CurrentMap;
            if (map != null && map.CanHaveFurniture)
                yield return new ComponentButton(menu,
                    new RectangleF(Vector2.Zero, new Size2(menu.Bounds.Width - 2, 6)), Locale.GetInterface("Place"),
                    (button, pressType) => {
                        if (button == MouseButton.Left && pressType == PressType.Pressed) {
                            InterfaceManager.Overlay.CursorItem = this;
                            slot.Items[slot.Index] = null;
                            InterfaceManager.SetInterface(null);
                            return true;
                        }
                        return false;
                    });
            foreach (var comp in base.GetContextMenu(slot, menu))
                yield return comp;
        }

        public void DrawPreview(SpriteBatch batch, Vector2 position, Color color, bool offset) {
            var tex = this.Type.Texture;
            var bounds = this.Type.RenderBounds;
            var pos = offset ? position + bounds.Position : position;
            batch.Draw(tex, pos, null, color, 0F, Vector2.Zero,
                new Vector2(bounds.Width / tex.Width, bounds.Height / tex.Height),
                SpriteEffects.None, 0);
        }

    }

    public class FurnitureType : ItemType {

        public readonly Texture2D Texture;
        public readonly RectangleF RenderBounds;
        public readonly RectangleF CollisionBounds;
        public readonly RectangleF PlacementBounds;
        public float DepthOffset;

        public FurnitureType(string name, RectangleF renderBounds, RectangleF collisionBounds, RectangleF? placementBounds = null)
            : base(name, new Point(0, 0)) {
            this.Texture = GameImpl.LoadContent<Texture2D>("Objects/Furniture/" + name);
            this.RenderBounds = renderBounds;
            this.CollisionBounds = collisionBounds;
            this.PlacementBounds = placementBounds ?? collisionBounds;
        }

        public FurnitureType SetDepthOffset(float offset) {
            this.DepthOffset = offset;
            return this;
        }

        public override Item Instance() {
            return new ItemFurniture(this);
        }

    }
}