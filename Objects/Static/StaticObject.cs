using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Objects.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Objects.Static {
    public class StaticObject : MapObject {

        private readonly Texture2D texture;
        public RectangleF IntersectionBounds;

        public StaticObject(string texture, Map map, Vector2 position) : base(map, position) {
            this.texture = texture != null ? GameImpl.LoadContent<Texture2D>("Objects/" + texture) : null;
        }

        public override void Draw(SpriteBatch batch) {
            batch.Draw(this.texture,
                this.Position + this.RenderBounds.Position,
                null, Color.White, 0F, Vector2.Zero,
                new Vector2(this.RenderBounds.Width / this.texture.Width, this.RenderBounds.Height / this.texture.Height),
                SpriteEffects.None, this.GetRenderDepth());
        }

        public virtual void OnIntersectWith(Character obj) {
        }

    }
}