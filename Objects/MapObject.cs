using System;
using AnimalTownGame.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Objects {
    public class MapObject {

        public readonly Map Map;
        public RectangleF RenderBounds;
        public RectangleF CollisionBounds;
        public Vector2 Position;

        public MapObject(Map map, Vector2 position) {
            this.Map = map;
            this.Position = position;
        }

        public virtual void UpdateRealTime(DateTime now, DateTime lastUpdate, TimeSpan passed) {
        }

        public virtual void Draw(SpriteBatch batch) {
        }

        public float GetRenderDepth() {
            return Math.Max(0, this.Position.Y / this.Map.HeightInTiles / 1000F);
        }

    }
}