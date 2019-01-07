using AnimalTownGame.Main;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimalTownGame.Maps {
    public class AutoTextureTile : Tile {

        private readonly AutoTextureType type;
        private Point topLeftCoord;
        private Point topRightCoord;
        private Point bottomLeftCoord;
        private Point bottomRightCoord;

        public AutoTextureTile(AutoTextureType type, Map map, Point position) : base(type, map, position) {
            this.type = type;
            this.OnNeighborChanged(position);
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

            var topLeft = !inner || this.IsSame(Direction.UpLeft.Offset);
            var topRight = !inner || this.IsSame(Direction.UpRight.Offset);
            var bottomLeft = !inner || this.IsSame(Direction.DownLeft.Offset);
            var bottomRight = !inner || this.IsSame(Direction.DownRight.Offset);

            var tl = new Point(0, 0);
            var tr = new Point(2, 0);
            var bl = new Point(0, 2);
            var br = new Point(2, 2);

            if (up && down && left && right)
                if (!topLeft && !topRight && !bottomLeft && !bottomRight) {
                    tl = new Point(4, 1);
                    bl = new Point(4, 0);
                    tr = new Point(3, 1);
                    br = new Point(3, 0);
                } else if (!topLeft && !topRight && !bottomLeft) {
                    tl = new Point(4, 1);
                    bl = new Point(4, 0);
                    tr = new Point(3, 1);
                    br = new Point(1, 1);
                } else if (!topLeft && !bottomLeft && !bottomRight) {
                    tl = new Point(4, 1);
                    bl = new Point(4, 0);
                    br = new Point(3, 0);
                    tr = new Point(1, 1);
                } else if (!topLeft && !topRight && !bottomRight) {
                    tl = new Point(4, 1);
                    tr = new Point(3, 1);
                    br = new Point(3, 0);
                    bl = new Point(1, 1);
                } else if (!bottomLeft && !bottomRight && !topRight) {
                    tr = new Point(3, 1);
                    bl = new Point(4, 0);
                    br = new Point(3, 0);
                    tl = new Point(1, 1);
                } else if (!topLeft && !bottomLeft) {
                    tl = new Point(4, 1);
                    bl = new Point(4, 0);
                    tr = br = new Point(1, 1);
                } else if (!topRight && !bottomRight) {
                    tr = new Point(3, 1);
                    br = new Point(3, 0);
                    tl = bl = new Point(1, 1);
                } else if (!bottomLeft && !bottomRight) {
                    bl = new Point(4, 0);
                    br = new Point(3, 0);
                    tl = tr = new Point(1, 1);
                } else if (!topLeft && !topRight) {
                    tl = new Point(4, 1);
                    tr = new Point(3, 1);
                    bl = br = new Point(1, 1);
                } else if (!topLeft && !bottomRight) {
                    tl = new Point(4, 1);
                    br = new Point(3, 0);
                    tr = bl = new Point(1, 1);
                } else if (!bottomLeft && !topRight) {
                    tr = new Point(3, 1);
                    bl = new Point(4, 0);
                    tl = br = new Point(1, 1);
                } else if (!topLeft)
                    tl = tr = bl = br = new Point(4, 1);
                else if (!topRight)
                    tl = tr = bl = br = new Point(3, 1);
                else if (!bottomLeft)
                    tl = tr = bl = br = new Point(4, 0);
                else if (!bottomRight)
                    tl = tr = bl = br = new Point(3, 0);
                else
                    tl = tr = bl = br = new Point(1, 1);

            else if (up && down && right)
                if (!bottomRight && !topRight) {
                    tl = bl = new Point(0, 1);
                    tr = new Point(3, 1);
                    br = new Point(3, 0);
                } else if (!topRight) {
                    tl = bl = new Point(0, 1);
                    tr = new Point(3, 1);
                    br = new Point(1, 1);
                } else if (!bottomRight) {
                    tl = bl = new Point(0, 1);
                    br = new Point(3, 0);
                    tr = new Point(1, 1);
                } else
                    tl = tr = bl = br = new Point(0, 1);
            else if (up && down && left)
                if (!bottomLeft && !topLeft) {
                    tr = br = new Point(2, 1);
                    tl = new Point(4, 1);
                    bl = new Point(4, 0);
                } else if (!topLeft) {
                    tr = br = new Point(2, 1);
                    tl = new Point(4, 1);
                    bl = new Point(1, 1);
                } else if (!bottomLeft) {
                    tr = br = new Point(2, 1);
                    bl = new Point(4, 0);
                    tl = new Point(1, 1);
                } else
                    tl = tr = bl = br = new Point(2, 1);
            else if (down && left && right)
                if (!bottomLeft && !bottomRight) {
                    tl = tr = new Point(1, 0);
                    bl = new Point(4, 0);
                    br = new Point(3, 0);
                } else if (!bottomLeft) {
                    tl = tr = new Point(1, 0);
                    bl = new Point(4, 0);
                    br = new Point(1, 1);
                } else if (!bottomRight) {
                    tl = tr = new Point(1, 0);
                    br = new Point(3, 0);
                    bl = new Point(1, 1);
                } else
                    tl = tr = bl = br = new Point(1, 0);
            else if (up && left && right)
                if (!topLeft && !topRight) {
                    bl = br = new Point(1, 2);
                    tr = new Point(3, 1);
                    tl = new Point(4, 1);
                } else if (!topRight) {
                    bl = br = new Point(1, 2);
                    tr = new Point(3, 1);
                    tl = new Point(1, 1);
                } else if (!topLeft) {
                    bl = br = new Point(1, 2);
                    tl = new Point(4, 1);
                    tr = new Point(1, 1);
                } else
                    tl = tr = bl = br = new Point(1, 2);

            else if (down && right)
                if (!bottomRight) {
                    tl = tr = bl = new Point(0, 0);
                    br = new Point(3, 0);
                } else
                    tl = tr = bl = br = new Point(0, 0);
            else if (down && left)
                if (!bottomLeft) {
                    tl = tr = br = new Point(2, 0);
                    bl = new Point(4, 0);
                } else
                    tl = tr = bl = br = new Point(2, 0);
            else if (up && right)
                if (!topRight) {
                    tl = bl = br = new Point(0, 2);
                    tr = new Point(3, 1);
                } else
                    tl = tr = bl = br = new Point(0, 2);
            else if (up && left)
                if (!topLeft) {
                    tr = br = bl = new Point(2, 2);
                    tl = new Point(4, 1);
                } else
                    tl = tr = bl = br = new Point(2, 2);
            else if (up && down) {
                tl = bl = new Point(0, 1);
                tr = br = new Point(2, 1);
            } else if (left && right) {
                tl = tr = new Point(1, 0);
                bl = br = new Point(1, 2);
            } else if (up) {
                tl = bl = new Point(0, 2);
                tr = br = new Point(2, 2);
            } else if (down) {
                tl = bl = new Point(0, 0);
                tr = br = new Point(2, 0);
            } else if (left) {
                tl = tr = new Point(2, 0);
                bl = br = new Point(2, 2);
            } else if (right) {
                tl = tr = new Point(0, 0);
                bl = br = new Point(0, 2);
            }

            this.topLeftCoord = this.Type.TextureCoord + tl;
            this.topRightCoord = this.Type.TextureCoord + tr;
            this.bottomLeftCoord = this.Type.TextureCoord + bl;
            this.bottomRightCoord = this.Type.TextureCoord + br;
        }

        private bool IsSame(Point offset) {
            var tile = this.Map[this.Position + offset];
            return tile == null || tile.Type == this.Type;
        }

        public override void Draw(SpriteBatch batch) {
            var atlas = Registry.TextureOutside;
            var topLeft = atlas.GetRegion(this.topLeftCoord.X, this.topLeftCoord.Y);
            batch.Draw(
                atlas.Texture,
                this.Position.ToVector2(),
                new Rectangle(topLeft.X, topLeft.Y, topLeft.Width / 2, topLeft.Height / 2),
                Color.White, 0F, Vector2.Zero,
                new Vector2(1F / topLeft.Width, 1F / topLeft.Height), SpriteEffects.None, 0);
            var topRight = atlas.GetRegion(this.topRightCoord.X, this.topRightCoord.Y);
            batch.Draw(
                atlas.Texture,
                this.Position.ToVector2() + new Vector2(0.5F, 0F),
                new Rectangle(topRight.X + topRight.Width / 2, topRight.Y, topRight.Width / 2, topRight.Height / 2),
                Color.White, 0F, Vector2.Zero,
                new Vector2(1F / topRight.Width, 1F / topRight.Height), SpriteEffects.None, 0);
            var bottomLeft = atlas.GetRegion(this.bottomLeftCoord.X, this.bottomLeftCoord.Y);
            batch.Draw(
                atlas.Texture,
                this.Position.ToVector2() + new Vector2(0F, 0.5F),
                new Rectangle(bottomLeft.X, bottomLeft.Y + bottomLeft.Height / 2, bottomLeft.Width / 2, bottomLeft.Height / 2),
                Color.White, 0F, Vector2.Zero,
                new Vector2(1F / bottomLeft.Width, 1F / bottomLeft.Height), SpriteEffects.None, 0);
            var bottomRight = atlas.GetRegion(this.bottomRightCoord.X, this.bottomRightCoord.Y);
            batch.Draw(
                atlas.Texture,
                this.Position.ToVector2() + new Vector2(0.5F, 0.5F),
                new Rectangle(bottomRight.X + bottomRight.Width / 2, bottomRight.Y + bottomRight.Height / 2, bottomRight.Width / 2, bottomRight.Height / 2),
                Color.White, 0F, Vector2.Zero,
                new Vector2(1F / bottomRight.Width, 1F / bottomRight.Height), SpriteEffects.None, 0);
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