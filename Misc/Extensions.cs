using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimalTownGame.Misc {
    public static class Extensions {

        public static int Floor(this float value) {
            var i = (int) value;
            return value < (float) i ? i - 1 : i;
        }

        public static int Ceil(this float value) {
            var i = (int) value;
            return value > (float) i ? i + 1 : i;
        }

        public static float DistanceSquared(this Point p1, Point value2) {
            float num1 = p1.X - value2.X;
            float num2 = p1.Y - value2.Y;
            return num1 * num1 + num2 * num2;
        }

        public static void DrawCenteredString(this SpriteBatch batch, SpriteFont spriteFont, string text, Vector2 position, bool hor, bool vert, Color color, float scale) {
            var data = spriteFont.MeasureString(text) * scale;
            batch.DrawString(spriteFont, text,
                position - new Vector2(hor ? data.X / 2 : 0, vert ? data.Y / 2 : 0),
                color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

    }
}