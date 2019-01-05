namespace AnimalTownGame.Misc {
    public static class Util {

        public static int Floor(double value) {
            var i = (int) value;
            return value < (double) i ? i - 1 : i;
        }

        public static int Ceil(double value) {
            var i = (int) value;
            return value > (double) i ? i + 1 : i;
        }

    }
}