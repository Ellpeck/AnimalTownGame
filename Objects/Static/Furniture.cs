using AnimalTownGame.Items;
using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AnimalTownGame.Objects.Static {
    public class Furniture : StaticObject {

        public readonly FurnitureType Type;

        public Furniture(FurnitureType type, Map map, Vector2 position) : base(type.Texture, map, position) {
            this.CollisionBounds = type.CollisionBounds;
            this.RenderBounds = type.RenderBounds;
            this.HighlightBounds = this.RenderBounds;
            this.DepthOffset = type.DepthOffset;
            this.Type = type;
        }

        public override bool OnMouse(Vector2 pos, MouseButton button, PressType type) {
            if (button == MouseButton.Right && type == PressType.Pressed) {
                if (!this.IsCovered()) {
                    InterfaceManager.Overlay.CursorItem = this.Type.Instance();
                    this.Map.StaticObjects.Remove(this);
                    return true;
                }
            }
            return false;
        }

        public bool IsCovered() {
            var myBounds = this.Type.PlacementBounds.Move(this.Position);
            foreach (var obj in this.Map.StaticObjects) {
                if (obj == this)
                    continue;
                var bounds = obj.CollisionBounds.Move(obj.Position);
                if (myBounds.IntersectsNonEmpty(bounds))
                    return true;
            }
            return false;
        }

    }
}