using AnimalTownGame.Interfaces;
using AnimalTownGame.Items;
using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimalTownGame.Objects.Static {
    public class Furniture : StaticObject {

        public readonly Item[] Storage;
        public readonly FurnitureType Type;
        public readonly Direction Direction;

        public Furniture(FurnitureType type, Map map, Vector2 position, Direction direction) : base(type.Texture, map, position) {
            this.CollisionBounds = type.CollisionBounds[direction];
            this.RenderBounds = type.RenderBounds;
            this.HighlightBounds = type.HighlightBounds[direction];
            this.DepthOffset = type.DepthOffset;
            this.Type = type;
            this.Direction = direction;
            if (type.IsStorage)
                this.Storage = new Item[24];
        }

        public override bool OnMouse(Vector2 pos, MouseButton button, PressType type) {
            if (button == MouseButton.Right && this.CanPickUp()) {
                if (type == PressType.Pressed) {
                    InterfaceManager.Overlay.CursorItem = this.Type.Instance();
                    InterfaceManager.Overlay.PlacementDirection = this.Direction;
                    this.Map.Objects.Remove(this);
                    return true;
                } else
                    InterfaceManager.SetCursorType(CursorType.Pick, 1F);
            } else if (button == MouseButton.Left && this.Storage != null) {
                var player = GameImpl.Instance.Player;
                var closeEnough = Vector2.DistanceSquared(player.Position,
                                      this.Position + this.CollisionBounds.Position + (Vector2) this.CollisionBounds.Size / 2F) <= 1;
                if (closeEnough && type == PressType.Pressed) {
                    InterfaceManager.SetInterface(new Storage(player, this));
                    return true;
                } else
                    InterfaceManager.SetCursorType(CursorType.Chest, closeEnough ? 1 : 0.5F);
            }
            return false;
        }

        public bool CanPickUp() {
            var myBounds = this.Type.PlacementBounds[this.Direction].Move(this.Position);
            foreach (var obj in this.Map.Objects) {
                if (obj == this)
                    continue;
                var bounds = obj.CollisionBounds.Move(obj.Position);
                if (myBounds.IntersectsNonEmpty(bounds))
                    return false;
            }

            if (this.Storage != null)
                foreach (var item in this.Storage)
                    if (item != null)
                        return false;

            return true;
        }

        public override void Draw(SpriteBatch batch) {
            this.Type.Draw(batch, this.Position + this.RenderBounds.Position, Color.White * this.ColorMod, this.GetRenderDepth(), this.Direction);
        }

    }
}