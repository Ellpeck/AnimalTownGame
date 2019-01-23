using System;
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
        public Direction PlacementDirection = Direction.Down;

        public Overlay() : base(GameImpl.Instance.Player) {
            var view = GameImpl.Instance.GraphicsDevice.Viewport;
            this.Init(view, new Size2(view.Width, view.Height) / InterfaceManager.Scale);
        }

        public override bool OnScroll(float scroll) {
            var furniture = this.CursorItem as ItemFurniture;
            if (furniture != null && furniture.Type.Rotates) {
                for (var i = 0; i < Direction.Adjacents.Length; i++)
                    if (Direction.Adjacents[i] == this.PlacementDirection) {
                        var newDir = i + (scroll > 0 ? 1 : -1);
                        if (newDir < 0)
                            newDir = 3;
                        else if (newDir > 3)
                            newDir = 0;
                        this.PlacementDirection = Direction.Adjacents[newDir];
                        break;
                    }
                return true;
            }
            return false;
        }

        public override bool OnMouse(MouseButton button, PressType type) {
            if (this.CursorItem != null && type == PressType.Pressed) {
                if (button == MouseButton.Right) {
                    this.OnClose();
                    return true;
                } else if (button == MouseButton.Left) {
                    var furniture = this.CursorItem as ItemFurniture;
                    if (furniture != null) {
                        var pos = GameImpl.Instance.Camera.ToWorldPos(Mouse.GetState().Position.ToVector2()).Floor() + Vector2.One * 0.5F;
                        if (!MapObject.IsCollidingPos(this.Player.Map, pos, furniture.Type.PlacementBounds[this.PlacementDirection])) {
                            var obj = new Furniture(furniture.Type, this.Player.Map, pos, this.PlacementDirection);
                            this.Player.Map.AddObject(obj);
                            this.CursorItem = null;
                            this.PlacementDirection = Direction.Down;
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
                GameImpl.CurrentTime.ToString(Locale.GetInterface("TimeHour")),
                this.position + this.timeOffset, true, true, Color.White, 0.325F);
            batch.DrawCenteredString(InterfaceManager.NormalFont,
                GameImpl.CurrentTime.ToString(Locale.GetInterface("TimeDay")),
                this.position + this.dateOffset, true, true, Color.White, 0.15F);
            base.Draw(batch);
        }

        public void DrawPreviews(SpriteBatch batch, Camera camera) {
            var furniture = this.CursorItem as ItemFurniture;
            if (furniture != null) {
                var pos = camera.ToWorldPos(Mouse.GetState().Position.ToVector2()).Floor() + Vector2.One * 0.5F;
                var color = MapObject.IsCollidingPos(this.Player.Map, pos, furniture.Type.PlacementBounds[this.PlacementDirection]) ? Color.Red : Color.White;
                furniture.DrawPreview(batch, pos, new Color(color, 0.25F), true, this.PlacementDirection);
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
                var inv = this.Player.Inventory;
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