using System;
using AnimalTownGame.Items;
using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using AnimalTownGame.Objects.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Objects {
    public class ItemObject : DynamicObject {

        private static readonly RectangleF Bounds = new RectangleF(-0.5F, -0.5F, 1, 1);

        public readonly Item Item;
        public float DestinationY;

        public ItemObject(Item item, Map map, Vector2 position) : base(map, position) {
            this.Item = item;
            this.DepthOffset = -0.25F;
            this.VelocityDamper = new Vector2(0.99F, 1F);
            this.NoClip = true;
            this.RenderBounds = Bounds;
            this.HighlightBounds = Bounds;
        }

        public override void Update(GameTime gameTime, bool isCurrent) {
            if (this.DestinationY != 0) {
                if (this.Position.Y < this.DestinationY) {
                    this.Velocity.Y += 0.0025F;
                    this.DepthOffset = -(this.Position.Y - this.DestinationY);
                } else {
                    this.Velocity = Vector2.Zero;
                    this.DestinationY = 0;
                    this.DepthOffset = -0.25F;
                }
            }

            base.Update(gameTime, isCurrent);
        }

        public override void Draw(SpriteBatch batch) {
            this.Item.Draw(batch, this.Position - Vector2.One * 0.5F, 1F / 16F, this.GetRenderDepth(), false);
        }

        public override bool OnMouse(Vector2 pos, MouseButton button, PressType type) {
            if (button == MouseButton.Right && this.DestinationY == 0) {
                var game = GameImpl.Instance;
                var closeEnough = Vector2.DistanceSquared(game.Player.Position, this.Position) <= 1;
                InterfaceManager.SetCursorType(CursorType.Pick, closeEnough ? 1F : 0.5F);
                if (closeEnough && type == PressType.Pressed) {
                    var inv = GameImpl.Instance.Player.Inventory;
                    for (var i = 0; i < inv.Length; i++)
                        if (inv[i] == null) {
                            inv[i] = this.Item;
                            this.Map.Objects.Remove(this);
                            return true;
                        }
                }
            }
            return false;
        }

        public static Vector2 GetFeasibleDropPos(Map map, Vector2 pos) {
            var realPos = pos.Floor() + Vector2.One * 0.5F;
            foreach (var dir in Direction.Values) {
                var offset = realPos + dir.Offset.ToVector2();
                if (IsFeasibleDropPos(map, offset))
                    return offset;
            }
            return Vector2.Zero;
        }

        private static bool IsFeasibleDropPos(Map map, Vector2 pos) {
            if (MapObject.IsCollidingPos(map, pos, Bounds, obj => !(obj is Player)))
                return false;
            var offsetBounds = Bounds.Move(pos);
            foreach (var obj in map.GetObjectsInArea(offsetBounds, obj => obj.RenderBounds, obj => obj is ItemObject)) {
                var objBounds = obj.RenderBounds.Move(obj.Position);
                if (objBounds.IntersectsNonEmpty(offsetBounds))
                    return false;
            }
            return true;
        }

    }
}