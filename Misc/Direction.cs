using Microsoft.Xna.Framework;

namespace AnimalTownGame.Misc {
    public class Direction {

        public static readonly Direction None = new Direction("None", 0, 0);
        public static readonly Direction Up = new Direction("Up", 0, -1);
        public static readonly Direction Down = new Direction("Down", 0, 1);
        public static readonly Direction Left = new Direction("Left", -1, 0);
        public static readonly Direction Right = new Direction("Right", 1, 0);
        public static readonly Direction UpLeft = new Direction("UpLeft", -1, -1);
        public static readonly Direction UpRight = new Direction("UpRight", 1, -1);
        public static readonly Direction DownLeft = new Direction("DownLeft", -1, 1);
        public static readonly Direction DownRight = new Direction("DownRight", 1, 1);
        public static readonly Direction[] Adjacents = {Up, Right, Down, Left};
        public static readonly Direction[] Arounds = {Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft};
        public static readonly Direction[] Values = {None, Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft};

        public readonly string Name;
        public readonly Point Offset;

        public Direction(string name, int offsetX, int offsetY) {
            this.Name = name;
            this.Offset = new Point(offsetX, offsetY);
        }

        public static Direction FromName(string name) {
            foreach (var dir in Values)
                if (dir.Name == name)
                    return dir;
            return null;
        }

    }
}