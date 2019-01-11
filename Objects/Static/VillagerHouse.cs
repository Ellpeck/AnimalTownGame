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
        private readonly Vector2 destPos;

        public VillagerHouse(string texture, Map map, Vector2 position, string destination, Vector2 destPos) : base("Houses/" + texture, map, position) {
            this.RenderBounds = new RectangleF(0, -5, 5, 6);
            this.CollisionBounds = new RectangleF(0.2F, -1, 4.6F, 2);
            this.HighlightBounds = new RectangleF(2, -1 - 3 / 16F, 1, 2);
            this.destination = destination;
            this.destPos = destPos;
        }

        public override void OnMouseOver(Vector2 mousePos) {
            InputManager.SetCursorType(CursorType.Door, 1F);
            if (InputManager.GetLeftMouse() == PressType.Pressed) {
                CutsceneManager.Fade(0.03F, () => {
                    var game = GameImpl.Instance;
                    game.Player.Teleport(game.Maps[this.destination], this.destPos);
                    game.Player.StopAndFace(Direction.Up);
                    CutsceneManager.Fade(-0.03F);
                });
            }
        }

    }
}