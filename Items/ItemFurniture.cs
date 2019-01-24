using System.Collections.Generic;
using AnimalTownGame.Interfaces.Components;
using AnimalTownGame.Main;
using AnimalTownGame.Misc;
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
                    new RectangleF(Vector2.Zero, new Size2(20, 6)), Locale.GetInterface("Place"),
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

        public void DrawPreview(SpriteBatch batch, Vector2 position, Color color, bool offset, Direction direction) {
            var pos = offset ? position + this.Type.RenderBounds.Position : position;
            this.Type.Draw(batch, pos, color, 0, direction);
        }

    }

    public class FurnitureType : ItemType {

        public readonly Texture2D Texture;
        public readonly RectangleF RenderBounds;
        public readonly Dictionary<Direction, RectangleF> CollisionBounds = new Dictionary<Direction, RectangleF>();
        public readonly Dictionary<Direction, RectangleF> PlacementBounds = new Dictionary<Direction, RectangleF>();
        public readonly Dictionary<Direction, RectangleF> HighlightBounds = new Dictionary<Direction, RectangleF>();
        public float DepthOffset;
        public bool IsStorage;
        public bool Rotates;

        public FurnitureType(string name, RectangleF renderBounds, RectangleF collisionBounds, RectangleF? highlightBounds = null, RectangleF? placementBounds = null)
            : base(name, new Point(0, 0)) {
            this.Texture = GameImpl.LoadContent<Texture2D>("Objects/Furniture/" + name);
            this.RenderBounds = renderBounds;
            foreach (var dir in Direction.Values) {
                this.CollisionBounds[dir] = collisionBounds;
                this.PlacementBounds[dir] = placementBounds ?? collisionBounds;
                this.HighlightBounds[dir] = highlightBounds ?? renderBounds;
            }
        }

        public FurnitureType SetDepthOffset(float offset) {
            this.DepthOffset = offset;
            return this;
        }

        public FurnitureType SetStorage() {
            this.IsStorage = true;
            return this;
        }

        public FurnitureType Bounds(RectangleF collisionBounds, RectangleF? highlightBounds, RectangleF? placementBounds, params Direction[] dirs) {
            this.Rotates = true;
            foreach (var dir in dirs) {
                this.CollisionBounds[dir] = collisionBounds;
                this.PlacementBounds[dir] = placementBounds ?? collisionBounds;
                if (highlightBounds.HasValue)
                    this.HighlightBounds[dir] = highlightBounds.Value;
            }
            return this;
        }

        public override Item Instance() {
            return new ItemFurniture(this);
        }

        public void Draw(SpriteBatch batch, Vector2 pos, Color color, float depth, Direction direction) {
            if (!this.Rotates) {
                batch.Draw(this.Texture,
                    pos, null, color, 0F, Vector2.Zero,
                    new Vector2(this.RenderBounds.Width / this.Texture.Width, this.RenderBounds.Height / this.Texture.Height),
                    SpriteEffects.None, depth);
            } else {
                var srcRect = new Rectangle(
                    direction == Direction.Right || direction == Direction.Up ? this.Texture.Width / 2 : 0,
                    direction == Direction.Left || direction == Direction.Right ? this.Texture.Height / 2 : 0,
                    this.Texture.Width / 2, this.Texture.Height / 2);
                batch.Draw(this.Texture,
                    pos, srcRect, color, 0F, Vector2.Zero,
                    new Vector2(this.RenderBounds.Width / (this.Texture.Width / 2), this.RenderBounds.Height / (this.Texture.Height / 2)),
                    SpriteEffects.None, depth);
            }
        }

    }
}