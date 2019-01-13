using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Objects.Static {
    public class Tree : StaticObject {

        private static readonly Texture2D Tex = GameImpl.LoadContent<Texture2D>("Objects/Tree");

        public Tree(Map map, Point position) : base(Tex, map, position.ToVector2() + Vector2.One * 0.5F) {
            this.RenderBounds = new RectangleF(-1.5F, -3.5F, 3, 4);
        }

    }
}