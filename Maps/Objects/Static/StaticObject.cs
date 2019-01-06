using AnimalTownGame.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimalTownGame.Maps.Objects.Static {
    public class StaticObject : MapObject {

        private readonly Texture2D texture;

        public StaticObject(string texture, Map map, Vector2 position) : base(map, position) {
            this.texture = GameImpl.LoadContent<Texture2D>("Objects/" + texture);
        }

        public override void Draw(SpriteBatch batch) {
            batch.Draw(this.texture,
                this.Position + this.RenderBounds.Position,
                null, Color.White, 0F, Vector2.Zero,
                new Vector2(this.RenderBounds.Width / this.texture.Width, this.RenderBounds.Height / this.texture.Height),
                SpriteEffects.None, this.GetRenderDepth());
        }

    }
}