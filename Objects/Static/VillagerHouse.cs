using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AnimalTownGame.Objects.Static {
    public class VillagerHouse : StaticObject {

        public static readonly string[] Textures = {
            "WhiteWood", "RedBrick"
        };

        private readonly string destination;

        public VillagerHouse(string texture, Map map, Vector2 position, string destination) : base("Houses/" + texture, map, position) {
            this.RenderBounds = new RectangleF(0, -5, 5, 6);
            this.CollisionBounds = new RectangleF(0.2F, -2F, 4.6F, 3F);
            this.HighlightBounds = new RectangleF(2, -1 - 3 / 16F, 1, 2);
            this.destination = destination;
        }

        public override void OnMouseOver(Vector2 mousePos) {
            var game = GameImpl.Instance;
            var closeEnough = Vector2.DistanceSquared(game.Player.Position, this.Position + new Vector2(2.5F, 0)) <= 2 * 2;
            InterfaceManager.SetCursorType(CursorType.Door, closeEnough ? 1F : 0.5F);
            if (closeEnough && InputManager.GetMouseType(0) == PressType.Pressed) {
                CutsceneManager.Fade(0.03F, () => {
                    var map = game.Maps[this.destination];
                    game.Player.Teleport(map, map.EntryPoint);
                    game.Player.StopAndFace(Direction.Up);
                    CutsceneManager.Fade(-0.03F);
                });
            }
        }

    }
}