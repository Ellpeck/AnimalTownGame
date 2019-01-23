using AnimalTownGame.Interfaces;
using AnimalTownGame.Items;
using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Objects.Static {
    public class Furniture : StaticObject {

        public readonly Item[] Storage;
        public readonly FurnitureType Type;

        public Furniture(FurnitureType type, Map map, Vector2 position) : base(type.Texture, map, position) {
            this.CollisionBounds = type.CollisionBounds;
            this.RenderBounds = type.RenderBounds;
            this.HighlightBounds = this.RenderBounds;
            this.DepthOffset = type.DepthOffset;
            this.Type = type;
            if (type.IsStorage)
                this.Storage = new Item[24];
        }

        public override bool OnMouse(Vector2 pos, MouseButton button, PressType type) {
            if (button == MouseButton.Right && this.CanPickUp()) {
                InterfaceManager.SetCursorType(CursorType.Pick, 1F);
                if (type == PressType.Pressed) {
                    InterfaceManager.Overlay.CursorItem = this.Type.Instance();
                    this.Map.Objects.Remove(this);
                    return true;
                }
            } else if (button == MouseButton.Left && this.Storage != null) {
                var player = GameImpl.Instance.Player;
                var closeEnough = Vector2.DistanceSquared(player.Position, this.Position) <= 2 * 2;
                InterfaceManager.SetCursorType(CursorType.Chest, closeEnough ? 1 : 0.5F);
                if (closeEnough && type == PressType.Pressed) {
                    InterfaceManager.SetInterface(new Storage(player, this));
                    return true;
                }
            }
            return false;
        }

        public bool CanPickUp() {
            var myBounds = this.Type.PlacementBounds.Move(this.Position);
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

    }
}