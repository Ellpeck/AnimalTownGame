using AnimalTownGame.Items;
using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AnimalTownGame.Objects.Static {
    public class Furniture : StaticObject {

        private readonly FurnitureType type;

        public Furniture(FurnitureType type, Map map, Vector2 position) : base(type.Texture, map, position) {
            this.CollisionBounds = type.CollisionBounds;
            this.RenderBounds = type.RenderBounds;
            this.HighlightBounds = this.RenderBounds;
            this.DepthOffset = type.DepthOffset;
            this.type = type;
        }

        public override bool OnMouse(Vector2 pos, MouseButton button, PressType type) {
            if (button == MouseButton.Right && type == PressType.Pressed) {
                if (!this.IsCovered()) {
                    InterfaceManager.Overlay.CursorItem = this.type.Instance();
                    this.Map.StaticObjects.Remove(this);
                    return true;
                }
            }
            return false;
        }

        public bool IsCovered() {
            var myBounds = this.type.PlacementBounds;
            myBounds.Offset(this.Position);
            foreach (var obj in this.Map.StaticObjects) {
                if (obj == this)
                    continue;
                var bounds = obj.CollisionBounds;
                if (bounds == RectangleF.Empty)
                    continue;
                bounds.Offset(obj.Position);
                if (myBounds.Intersects(bounds))
                    return true;
            }
            return false;
        }

    }
}