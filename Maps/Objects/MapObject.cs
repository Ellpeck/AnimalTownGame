using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Maps.Objects {
    public class MapObject {

        public readonly Map Map;
        public readonly Size2 Size;
        public Vector2 Position;

        public MapObject(Map map, Vector2 position, Size2 size) {
            this.Map = map;
            this.Position = position;
            this.Size = size;
        }

        public virtual void Update(TimeSpan passed) {
        }

        public virtual void Draw(SpriteBatch batch) {
        }

        public float GetRenderDepth() {
            return Math.Max(0, this.Position.Y / this.Map.HeightInTiles / 1000F);
        }

    }
}