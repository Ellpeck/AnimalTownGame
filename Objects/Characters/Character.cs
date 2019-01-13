using System;
using System.Collections.Generic;
using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.TextureAtlases;

namespace AnimalTownGame.Objects.Characters {
    public class Character : DynamicObject {

        protected readonly SpriteSheetAnimationFactory AnimationFactory;
        protected SpriteSheetAnimation CurrentAnimation;
        public Direction Direction = Direction.Down;
        public float WalkSpeed = 0.02F;

        public Character(string name, Map map, Vector2 position) : base(map, position) {
            this.RenderBounds = new RectangleF(-0.5F, -1.5F, 1, 2);
            this.CollisionBounds = new RectangleF(-0.4F, 0, 0.8F, 0.35F);

            this.AnimationFactory = new SpriteSheetAnimationFactory(new TextureAtlas(
                name,
                GameImpl.LoadContent<Texture2D>("Characters/" + name),
                GameImpl.LoadContent<Dictionary<string, Rectangle>>("Characters/Animation")));
            this.AnimationFactory.Add("StandingDown", new SpriteSheetAnimationData(new[] {0}));
            this.AnimationFactory.Add("Down", new SpriteSheetAnimationData(new[] {1, 2, 3}, isPingPong: true));
            this.AnimationFactory.Add("StandingUp", new SpriteSheetAnimationData(new[] {4}));
            this.AnimationFactory.Add("Up", new SpriteSheetAnimationData(new[] {5, 6, 7}, isPingPong: true));
            this.AnimationFactory.Add("StandingLeft", new SpriteSheetAnimationData(new[] {8}));
            this.AnimationFactory.Add("Left", new SpriteSheetAnimationData(new[] {9, 10, 11}, isPingPong: true));
            this.AnimationFactory.Add("StandingRight", new SpriteSheetAnimationData(new[] {12}));
            this.AnimationFactory.Add("Right", new SpriteSheetAnimationData(new[] {13, 14, 15}, isPingPong: true));
            this.CurrentAnimation = this.AnimationFactory.Create("StandingDown");
        }

        public override void Update(GameTime gameTime, bool isCurrent) {
            if (this.ShouldTurn()) {
                var limit = this.WalkSpeed / 2;
                if (Math.Abs(this.Velocity.X) >= limit)
                    this.Direction = this.Velocity.X > 0 ? Direction.Right : Direction.Left;
                if (Math.Abs(this.Velocity.Y) >= limit)
                    this.Direction = this.Velocity.Y > 0 ? Direction.Down : Direction.Up;
            }
            base.Update(gameTime, isCurrent);

            if (isCurrent) {
                this.UpdateAnimation();
                this.CurrentAnimation.Update(gameTime);
            }

            var myBounds = this.CollisionBounds;
            if (myBounds != RectangleF.Empty) {
                myBounds.Offset(this.Position);
                foreach (var obj in this.Map.StaticObjects) {
                    var bounds = obj.IntersectionBounds;
                    bounds.Offset(obj.Position);
                    if (myBounds.Intersects(bounds))
                        obj.OnIntersectWith(this);
                }
            }
        }

        public virtual bool ShouldTurn() {
            return true;
        }

        public override void Draw(SpriteBatch batch) {
            var frame = this.CurrentAnimation.CurrentFrame;
            batch.Draw(frame,
                this.Position + this.RenderBounds.Position,
                Color.White,
                0F, Vector2.Zero, new Vector2(this.RenderBounds.Width / frame.Width, this.RenderBounds.Height / frame.Height),
                SpriteEffects.None, this.GetRenderDepth());
        }

        protected void UpdateAnimation() {
            var toPlay = this.Direction.Name;
            var limit = this.WalkSpeed / 2;
            if (!(this is Player) || Math.Abs(this.Velocity.X) < limit && Math.Abs(this.Velocity.Y) < limit) {
                var diff = this.Position - this.LastPosition;
                if (Math.Abs(diff.X) < limit && Math.Abs(diff.Y) < limit)
                    toPlay = "Standing" + toPlay;
            }
            if (this.CurrentAnimation.Name != toPlay)
                this.CurrentAnimation = this.AnimationFactory.Create(toPlay);
        }

        public void StopMoving() {
            this.Velocity = Vector2.Zero;
        }

        public void StopAndFace(DynamicObject obj) {
            this.StopMoving();
            var diffX = obj.Position.X - this.Position.X;
            var diffY = obj.Position.Y - this.Position.Y;
            if (Math.Abs(diffX) > Math.Abs(diffY)) {
                if (diffX > 0)
                    this.Direction = Direction.Right;
                else if (diffX < 0)
                    this.Direction = Direction.Left;
            } else {
                if (diffY > 0)
                    this.Direction = Direction.Down;
                else if (diffY < 0)
                    this.Direction = Direction.Up;
            }
            this.UpdateAnimation();
        }

        public void StopAndFace(Direction direction) {
            this.StopMoving();
            this.Direction = direction;
            this.UpdateAnimation();
        }

    }
}