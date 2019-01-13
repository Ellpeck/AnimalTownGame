using AnimalTownGame.Maps;
using AnimalTownGame.Objects.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Objects.Static {
    public class StaticObject : MapObject {

        protected readonly Texture2D Texture;
        public RectangleF IntersectionBounds;

        public StaticObject(Texture2D texture, Map map, Vector2 position) : base(map, position) {
            this.Texture = texture;
        }

        public override void Draw(SpriteBatch batch) {
            batch.Draw(this.Texture,
                this.Position + this.RenderBounds.Position,
                null, Color.White, 0F, Vector2.Zero,
                new Vector2(this.RenderBounds.Width / this.Texture.Width, this.RenderBounds.Height / this.Texture.Height),
                SpriteEffects.None, this.GetRenderDepth());
        }

        public virtual void OnIntersectWith(Character obj) {
        }

    }
}