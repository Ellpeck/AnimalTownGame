using AnimalTownGame.Rendering;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Maps {
    public class WallTrim : AutoTextureTile {

        private bool hasTrimAbove;

        public WallTrim(AutoTextureType type, Map map, Point position) : base(type, map, position) {
        }

        public override void OnNeighborChanged(Point neighbor) {
            base.OnNeighborChanged(neighbor);
            var tileAbove = this.Map[this.Position - new Point(0, 1)];
            this.hasTrimAbove = tileAbove == null || tileAbove.Type == this.Type;
        }

        public override Rectangle GetCollisionBounds() {
            return this.hasTrimAbove ? base.GetCollisionBounds() : Rectangle.Empty;
        }

        public override float GetRenderDepth() {
            return 1 / 1000F;
        }

    }

    public class WallTrimType : AutoTextureType {

        public WallTrimType(Point textureCoord, TextureAtlas texture = null) : base(textureCoord, true, texture) {
        }

        public override Tile Instance(Map map, Point position) {
            return new WallTrim(this, map, position);
        }

    }
}