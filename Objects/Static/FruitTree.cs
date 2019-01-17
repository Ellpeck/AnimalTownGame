using System;
using AnimalTownGame.Items;
using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Objects.Static {
    public class FruitTree : StaticObject {

        public static readonly FruitType[] Types = {
            new FruitType("Apple", Registry.ItemApple, TimeSpan.FromDays(1.5))
        };

        private static readonly Vector2[] FruitOffsets = {
            new Vector2(0.2F, 0.5F),
            new Vector2(1F, 1F),
            new Vector2(1.8F, 0.5F)
        };

        public readonly FruitType Type;
        public TimeSpan FruitTimer;

        public FruitTree(FruitType type, Map map, Vector2 position) : base(type.Texture, map, position) {
            this.Type = type;
            this.FruitTimer = this.Type.Time;
            this.RenderBounds = new RectangleF(-1.5F, -3.5F, 3, 4);
            this.FadeBounds = new RectangleF(-1.5F, -3.5F, 3, 3);
            this.CollisionBounds = new RectangleF(-0.3F, -0.3F, 0.6F, 0.6F);
        }

        public override void UpdateRealTime(DateTime now, DateTime lastUpdate, TimeSpan passed) {
            if (this.FruitTimer > TimeSpan.Zero)
                this.FruitTimer -= passed;
        }

        public override void Draw(SpriteBatch batch) {
            var color = new Color(this.ColorMod, this.ColorMod, this.ColorMod, this.ColorMod);

            var treeHeight = this.Texture.Height * 0.8F;
            batch.Draw(this.Texture,
                this.Position + this.RenderBounds.Position,
                new Rectangle(0, 0, this.Texture.Width, (int) treeHeight), color, 0F, Vector2.Zero,
                new Vector2(this.RenderBounds.Width / this.Texture.Width, this.RenderBounds.Height / treeHeight),
                SpriteEffects.None, this.GetRenderDepth());

            var percentage = 1F - this.FruitTimer.Ticks / (float) this.Type.Time.Ticks;
            if (percentage < 0.3)
                return;

            var offset = 0;
            if (percentage >= 0.65)
                offset++;
            if (percentage >= 1)
                offset++;

            var srcWidth = this.Texture.Width / 3;
            var srcRect = new Rectangle(offset * srcWidth, (int) treeHeight,
                srcWidth, (int) (this.Texture.Height - treeHeight));

            for (var i = 0; i < FruitOffsets.Length; i++) {
                var fruitPos = FruitOffsets[i];
                batch.Draw(this.Texture,
                    this.Position + this.RenderBounds.Position + fruitPos,
                    srcRect, color, 0F, Vector2.Zero,
                    new Vector2(1F / srcRect.Width, 1F / srcRect.Height),
                    SpriteEffects.None, 1 / 1000F);
            }
        }

        public class FruitType {

            public readonly string Name;
            public readonly ItemType Drop;
            public readonly TimeSpan Time;
            public readonly Texture2D Texture;

            public FruitType(string name, ItemType drop, TimeSpan time) {
                this.Name = name;
                this.Drop = drop;
                this.Time = time;
                this.Texture = GameImpl.LoadContent<Texture2D>("Objects/Trees/" + name + "Tree");
            }

        }

    }
}