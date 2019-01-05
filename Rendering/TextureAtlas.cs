using AnimalTownGame.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimalTownGame.Rendering {
    public class TextureAtlas {

        public readonly int RegionWidth;
        public readonly int RegionHeight;
        public readonly Texture2D Texture;

        public TextureAtlas(string location, int regionWidth, int regionHeight) {
            this.RegionWidth = regionWidth;
            this.RegionHeight = regionHeight;
            this.Texture = GameImpl.Instance.Content.Load<Texture2D>(location);
        }

        public Rectangle GetRegion(int regionX, int regionY) {
            return new Rectangle(regionX * this.RegionWidth, regionY * this.RegionHeight, this.RegionWidth, this.RegionHeight);
        }

    }
}