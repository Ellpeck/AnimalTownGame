using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Objects.Static {
    public class VillagerHouse : StaticObject {

        public static readonly Texture2D[] Textures = {
            GameImpl.LoadContent<Texture2D>("Objects/Houses/WhiteWood"),
            GameImpl.LoadContent<Texture2D>("Objects/Houses/RedBrick")
        };

        private readonly string destination;

        public VillagerHouse(int textureIndex, Map map, Vector2 position, string destination) : base(Textures[textureIndex], map, position) {
            this.RenderBounds = new RectangleF(0, -5, 5, 6);
            this.FadeBounds = new RectangleF(0, -5, 5, 4);
            this.CollisionBounds = new RectangleF(0.2F, -2F, 4.6F, 3F);
            this.HighlightBounds = new RectangleF(2, -1 - 3 / 16F, 1, 2);
            this.destination = destination;
        }

        public override bool OnMouse(Vector2 pos, MouseButton button, PressType type) {
            if (button == MouseButton.Left) {
                var game = GameImpl.Instance;
                var closeEnough = Vector2.DistanceSquared(game.Player.Position, this.Position + new Vector2(2.5F, 0)) <= 2 * 2;
                if (closeEnough && type == PressType.Pressed) {
                    CutsceneManager.Fade(0.03F, () => {
                        var map = game.Maps[this.destination];
                        game.Player.Teleport(map, map.EntryPoint);
                        game.Player.StopAndFace(Direction.Up);
                        CutsceneManager.Fade(-0.03F);
                    });
                    return true;
                } else
                    InterfaceManager.SetCursorType(CursorType.Door, closeEnough ? 1F : 0.5F);
            }
            return false;
        }

    }
}