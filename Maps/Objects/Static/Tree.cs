using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AnimalTownGame.Maps.Objects.Static {
    public class Tree : StaticObject {

        public Tree(Map map, Vector2 position) : base("Tree", map, position) {
            this.RenderBounds = new RectangleF(-1.5F, -3.5F, 3, 4);
        }

    }
}