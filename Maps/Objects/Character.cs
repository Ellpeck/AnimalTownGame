using System;
using System.Collections.Generic;
using AnimalTownGame.Main;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;

namespace AnimalTownGame.Maps.Objects {
    public class Character : DynamicObject {

        protected readonly SpriteSheetAnimationFactory AnimationFactory;
        protected SpriteSheetAnimation CurrentAnimation;

        public Character(string name, Map map, Vector2 position) : base(map, position, new Size2(0.75F, 0.95F)) {
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

        public override void Update(TimeSpan passed) {
            base.Update(passed);

            this.UpdateAnimation();
            this.CurrentAnimation.Update((float) passed.TotalSeconds);
        }

        public override void Draw(SpriteBatch batch) {
            var frame = this.CurrentAnimation.CurrentFrame;
            batch.Draw(frame,
                this.Position - new Vector2(0.5F, 1.5F),
                Color.White,
                0F, Vector2.Zero, new Vector2(1F / frame.Width, 2F / frame.Height), SpriteEffects.None,
                this.GetRenderDepth());
        }

        protected void UpdateAnimation() {
            var toPlay = this.Direction.Name;
            if (!(this is Player) || Math.Abs(this.Velocity.X) < 0.01 && Math.Abs(this.Velocity.Y) < 0.01) {
                var diff = this.Position - this.LastPosition;
                if (Math.Abs(diff.X) < 0.01 && Math.Abs(diff.Y) < 0.01)
                    toPlay = "Standing" + toPlay;
            }
            if (this.CurrentAnimation.Name != toPlay)
                this.CurrentAnimation = this.AnimationFactory.Create(toPlay);
        }

    }
}