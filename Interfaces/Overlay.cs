using AnimalTownGame.Items;
using AnimalTownGame.Main;
using AnimalTownGame.Misc;
using AnimalTownGame.Objects;
using AnimalTownGame.Objects.Static;
using AnimalTownGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace AnimalTownGame.Interfaces {
    public class Overlay : ItemInterface {

        private static readonly Texture2D Texture = GameImpl.LoadContent<Texture2D>("Interfaces/Overlay");
        private Vector2 position;
        private Vector2 timeOffset;
        private Vector2 dateOffset;

        public Overlay() {
            var view = GameImpl.Instance.GraphicsDevice.Viewport;
            this.Init(view, new Size2(view.Width, view.Height) / InterfaceManager.Scale);
        }

        public override bool OnMouse(MouseButton button, PressType type) {
            if (this.CursorItem != null && type == PressType.Pressed) {
                if (button == MouseButton.Right) {
                    this.OnClose();
                    return true;
                } else if (button == MouseButton.Left) {
                    var furniture = this.CursorItem as ItemFurniture;
                    if (furniture != null) {
                        var game = GameImpl.Instance;
                        var pos = game.Camera.ToWorldPos(Mouse.GetState().Position.ToVector2()).Floor() + Vector2.One * 0.5F;
                        if (!MapObject.IsCollidingPos(game.CurrentMap, pos, furniture.Type.PlacementBounds, null)) {
                            var obj = new Furniture(furniture.Type, game.CurrentMap, pos, furniture.Type.CollisionBounds, furniture.Type.RenderBounds);
                            obj.DepthOffset = furniture.Type.DepthOffset;
                            game.CurrentMap.StaticObjects.Add(obj);
                            this.CursorItem = null;
                            return true;
                        }
                    }
                }
            }
            return base.OnMouse(button, type);
        }

        public override void Draw(SpriteBatch batch) {
            batch.Draw(Texture, this.position, Color.White);
            batch.DrawCenteredString(InterfaceManager.NormalFont,
                GameImpl.CurrentTime.ToString("h:mm tt"),
                this.position + this.timeOffset, true, true, Color.White, 0.325F);
            batch.DrawCenteredString(InterfaceManager.NormalFont,
                GameImpl.CurrentTime.ToString("dddd d MMM"),
                this.position + this.dateOffset, true, true, Color.White, 0.15F);
            base.Draw(batch);
        }

        public void DrawPreviews(SpriteBatch batch, Camera camera) {
            var furniture = this.CursorItem as ItemFurniture;
            if (furniture != null) {
                var map = GameImpl.Instance.CurrentMap;
                var pos = camera.ToWorldPos(Mouse.GetState().Position.ToVector2()).Floor() + Vector2.One * 0.5F;
                var color = MapObject.IsCollidingPos(map, pos, furniture.Type.PlacementBounds, null) ? Color.Red : Color.White;
                furniture.DrawPreview(batch, pos, new Color(color, 0.25F), true);
            }
        }

        public override void Init(Viewport viewport, Size2 viewportSize) {
            base.Init(viewport, viewportSize);
            this.position = new Vector2(2, 0);
            this.timeOffset = new Vector2(24, 12);
            this.dateOffset = new Vector2(24, 25.5F);
        }

        public override void OnClose() {
            if (this.CursorItem != null) {
                var inv = GameImpl.Instance.Player.Inventory;
                for (var i = 0; i < inv.Length; i++) {
                    if (inv[i] != null)
                        continue;
                    inv[i] = this.CursorItem;
                    this.CursorItem = null;
                    break;
                }
            }
        }

    }
}