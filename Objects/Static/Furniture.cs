using AnimalTownGame.Items;
using AnimalTownGame.Maps;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AnimalTownGame.Objects.Static {
    public class Furniture : StaticObject {

        public Furniture(FurnitureType type, Map map, Vector2 position, RectangleF collision, RectangleF render)
            : base(type.Texture, map, position) {
            this.CollisionBounds = collision;
            this.RenderBounds = render;
        }

    }
}