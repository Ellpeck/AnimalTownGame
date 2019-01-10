using AnimalTownGame.Maps;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AnimalTownGame.Objects.Static {
    public class VillagerHouse : StaticObject {

        public static readonly string[] Textures = {
            "WhiteWood", "RedBrick"
        };

        public VillagerHouse(string texture, Map map, Vector2 position) : base("Houses/" + texture, map, position) {
            this.RenderBounds = new RectangleF(0, -5, 5, 6);
            this.CollisionBounds = new RectangleF(0.2F, -0.5F, 4.6F, 2);
        }


    }
}