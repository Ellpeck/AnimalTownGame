using AnimalTownGame.Main;
using AnimalTownGame.Misc;
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
            var atlas = Registry.TextureOutside;
            var texCoord = this.Type.TextureCoord;
            batch.Draw(
                atlas.Texture,
                new Rectangle(this.Position.X, this.Position.Y, 1, 1),
                atlas.GetRegion(texCoord.X, texCoord.Y),
                Color.White);
        }

    }

    public class TileType {

        public readonly Point TextureCoord;
        public int PathCost = PathFinding.DefaultPathfindCost;

        public TileType(Point textureCoord) {
            this.TextureCoord = textureCoord;
        }

        public TileType SetCost(int cost) {
            this.PathCost = cost;
            return this;
        }

        public virtual Tile Instance(Map map, Point position) {
            return new Tile(this, map, position);
        }

    }
}