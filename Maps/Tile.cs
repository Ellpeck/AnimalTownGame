using AnimalTownGame.Main;
using AnimalTownGame.Misc;
using AnimalTownGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimalTownGame.Maps {
    public class Tile {

        public readonly TileType Type;
        public readonly Map Map;
        public readonly Point Position;

        public Tile(TileType type, Map map, Point position) {
            this.Type = type;
            this.Map = map;
            this.Position = position;
        }

        public virtual void OnNeighborChanged(Point neighbor) {
        }

        public virtual void Draw(SpriteBatch batch) {
            var atlas =this.Type.Texture;
            var texCoord = this.Type.TextureCoord;
            batch.Draw(
                atlas.Texture,
                new Rectangle(this.Position.X, this.Position.Y, 1, 1),
                atlas.GetRegion(texCoord.X, texCoord.Y),
                Color.White);
        }

        public virtual Rectangle GetCollisionBounds() {
            return this.Type.Walkability >= int.MaxValue ?
                new Rectangle(this.Position, new Point(1, 1)) : Rectangle.Empty;
        }
    }

    public class TileType {

        public readonly TextureAtlas Texture;
        public readonly Point TextureCoord;
        public int Walkability = PathFinding.DefaultPathfindCost;

        public TileType(Point textureCoord, TextureAtlas texture = null) {
            this.TextureCoord = textureCoord;
            this.Texture = texture ?? Registry.TextureOutside;
        }

        public TileType SetWalkability(int cost) {
            this.Walkability = cost;
            return this;
        }

        public virtual Tile Instance(Map map, Point position) {
            return new Tile(this, map, position);
        }

    }
}