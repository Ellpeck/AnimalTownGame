using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using AnimalTownGame.Objects.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Objects.Static {
    public class StaticObject : MapObject {

        protected readonly Texture2D Texture;
        protected RectangleF FadeBounds;
        protected float ColorMod = 1F;

        public StaticObject(Texture2D texture, Map map, Vector2 position) : base(map, position) {
            this.Texture = texture;
        }

        public override void Update(GameTime gameTime, bool isCurrent) {
            if (this.FadeBounds != RectangleF.Empty) {
                var myBounds = this.FadeBounds.Move(this.Position);
                if (myBounds.Contains(GameImpl.Instance.Player.Position)) {
                    if (this.ColorMod > 0.45F)
                        this.ColorMod -= 0.05F;
                } else {
                    if (this.ColorMod < 1F)
                        this.ColorMod += 0.05F;
                }
            }
        }

        public override void Draw(SpriteBatch batch) {
            batch.Draw(this.Texture,
                this.Position + this.RenderBounds.Position,
                null, new Color(this.ColorMod, this.ColorMod, this.ColorMod, this.ColorMod), 0F, Vector2.Zero,
                new Vector2(this.RenderBounds.Width / this.Texture.Width, this.RenderBounds.Height / this.Texture.Height),
                SpriteEffects.None, this.GetRenderDepth());
        }

    }
}