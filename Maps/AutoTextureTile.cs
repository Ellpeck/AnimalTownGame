using AnimalTownGame.Main;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimalTownGame.Maps {
    public class AutoTextureTile : Tile {

        private readonly AutoTextureType type;
        private Point currentTexCoord;

        public AutoTextureTile(AutoTextureType type, Map map, Point position) : base(type, map, position) {
            this.type = type;
            this.currentTexCoord = type.TextureCoord + new Point(3, 1);
        }

        public override void OnNeighborChanged(Point neighbor) {
            var inner = this.type.HasInnerCorners;
            if (!inner) {
                var diff = this.Position - neighbor;
                if (diff.X != 0 && diff.Y != 0)
                    return;
            }

            var up = this.IsSame(Direction.Up.Offset);
            var down = this.IsSame(Direction.Down.Offset);
            var left = this.IsSame(Direction.Left.Offset);
            var right = this.IsSame(Direction.Right.Offset);

            var topLeft = !inner || this.IsSame(new Point(-1, -1));
            var topRight = !inner || this.IsSame(new Point(1, -1));
            var bottomLeft = !inner || this.IsSame(new Point(-1, 1));
            var bottomRight = !inner || this.IsSame(new Point(1, 1));

            var add = new Point(5, 0);

            if (up && down && left && right)
                if (!topLeft && !topRight && !bottomLeft && !bottomRight)
                    add = new Point(0, 7);
                else if (!topLeft && !topRight && !bottomLeft)
                    add = new Point(5, 3);
                else if (!topLeft && !bottomLeft && !bottomRight)
                    add = new Point(5, 4);
                else if (!topLeft && !topRight && !bottomRight)
                    add = new Point(5, 5);
                else if (!bottomLeft && !bottomRight && !topRight)
                    add = new Point(5, 6);
                else if (!topLeft && !bottomLeft)
                    add = new Point(0, 5);
                else if (!topRight && !bottomRight)
                    add = new Point(1, 5);
                else if (!bottomLeft && !bottomRight)
                    add = new Point(0, 6);
                else if (!topLeft && !topRight)
                    add = new Point(1, 6);
                else if (!topLeft && !bottomRight)
                    add = new Point(5, 1);
                else if (!bottomLeft && !topRight)
                    add = new Point(5, 2);
                else if (!topLeft)
                    add = new Point(1, 4);
                else if (!topRight)
                    add = new Point(0, 4);
                else if (!bottomLeft)
                    add = new Point(1, 3);
                else if (!bottomRight)
                    add = new Point(0, 3);
                else
                    add = new Point(1, 1);

            else if (up && down && right)
                if (!bottomRight && !topRight)
                    add = new Point(4, 4);
                else if (!topRight)
                    add = new Point(3, 4);
                else if (!bottomRight)
                    add = new Point(2, 4);
                else
                    add = new Point(0, 1);
            else if (up && down && left)
                if (!bottomLeft && !topLeft)
                    add = new Point(4, 3);
                else if (!topLeft)
                    add = new Point(3, 3);
                else if (!bottomLeft)
                    add = new Point(2, 3);
                else
                    add = new Point(2, 1);
            else if (down && left && right)
                if (!bottomLeft && !bottomRight)
                    add = new Point(4, 5);
                else if (!bottomLeft)
                    add = new Point(3, 5);
                else if (!bottomRight)
                    add = new Point(2, 5);
                else
                    add = new Point(1, 0);
            else if (up && left && right)
                if (!topLeft && !topRight)
                    add = new Point(4, 6);
                else if (!topRight)
                    add = new Point(3, 6);
                else if (!topLeft)
                    add = new Point(2, 6);
                else
                    add = new Point(1, 2);

            else if (down && right)
                if (!bottomRight)
                    add = new Point(3, 7);
                else
                    add = Point.Zero;
            else if (down && left)
                if (!bottomLeft)
                    add = new Point(4, 7);
                else
                    add = new Point(2, 0);
            else if (up && right)
                if (!topRight)
                    add = new Point(1, 7);
                else
                    add = new Point(0, 2);
            else if (up && left)
                if (!topLeft)
                    add = new Point(2, 7);
                else
                    add = new Point(2, 2);
            else if (up && down)
                add = new Point(3, 0);
            else if (left && right)
                add = new Point(3, 1);

            else if (up)
                add = new Point(4, 1);
            else if (down)
                add = new Point(4, 0);
            else if (left)
                add = new Point(4, 2);
            else if (right)
                add = new Point(3, 2);

            this.currentTexCoord = this.Type.TextureCoord + add;
        }

        private bool IsSame(Point offset) {
            var tile = this.Map[this.Position + offset];
            return tile != null && tile.Type == this.Type;
        }

        public override void Draw(SpriteBatch batch) {
            var atlas = Registry.TextureOutside;
            batch.Draw(
                atlas.Texture,
                new Rectangle(this.Position.X, this.Position.Y, 1, 1),
                atlas.GetRegion(this.currentTexCoord.X, this.currentTexCoord.Y),
                Color.White);
        }

    }

    public class AutoTextureType : TileType {

        public readonly bool HasInnerCorners;

        public AutoTextureType(Point textureCoord, bool hasInnerCorners) : base(textureCoord) {
            this.HasInnerCorners = hasInnerCorners;
        }

        public override Tile Instance(Map map, Point position) {
            return new AutoTextureTile(this, map, position);
        }

    }
}