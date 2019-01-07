using Microsoft.Xna.Framework;

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

    }
}