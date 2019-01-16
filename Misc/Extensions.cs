using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Misc {
    public static class Extensions {

        public static int Floor(this float value) {
            var i = (int) value;
            return value < (float) i ? i - 1 : i;
        }

        public static Vector2 Floor(this Vector2 value) {
            return new Vector2(value.X.Floor(), value.Y.Floor());
        }

        public static int Ceil(this float value) {
            var i = (int) value;
            return value > (float) i ? i + 1 : i;
        }

        public static void DrawCenteredString(this SpriteBatch batch, SpriteFont spriteFont, string text, Vector2 position, bool hor, bool vert, Color color, float scale) {
            var data = spriteFont.MeasureString(text) * scale;
            batch.DrawString(spriteFont, text,
                position - new Vector2(hor ? data.X / 2 : 0, vert ? data.Y / 2 : 0),
                color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public static void DrawInfoBox(this SpriteBatch batch, SpriteFont spriteFont, string text, Vector2 position, float scale) {
            var data = spriteFont.MeasureString(text) * scale;
            batch.FillRectangle(position, new Size2(data.X + 2, data.Y + 2), new Color(Color.Black, 0.75F));
            batch.DrawString(spriteFont, text, position + Vector2.One, Color.White, 0F, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public static RectangleF Move(this RectangleF rect, Vector2 position) {
            if (rect == RectangleF.Empty)
                return rect;
            rect.Offset(position);
            return rect;
        }

        public static bool IntersectsNonEmpty(this RectangleF rect, RectangleF other) {
            if (rect == RectangleF.Empty || other == RectangleF.Empty)
                return false;
            return rect.Intersects(other);
        }

    }
}