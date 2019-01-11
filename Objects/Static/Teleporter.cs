using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using AnimalTownGame.Objects.Characters;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AnimalTownGame.Objects.Static {
    public class Teleporter : StaticObject {

        private readonly string destination;
        private readonly Vector2 destPos;
        private readonly Direction destDirection;

        public Teleporter(Map map, Vector2 position, string destination, Vector2 destPos, Direction destDirection) : base(null, map, position + Vector2.One / 2) {
            this.destination = destination;
            this.destPos = destPos;
            this.destDirection = destDirection;
            this.IntersectionBounds = new RectangleF(-0.5F, -0.5F, 1, 1);
        }

        public override void OnIntersectWith(Character obj) {
            CutsceneManager.Fade(0.03F, () => {
                obj.Teleport(GameImpl.Instance.Maps[this.destination], this.destPos);
                obj.StopAndFace(this.destDirection);
                CutsceneManager.Fade(-0.03F);
            });
        }

    }
}