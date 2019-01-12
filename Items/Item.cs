using AnimalTownGame.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Items {
    public class Item {

        public readonly ItemType Type;

        public Item(ItemType type) {
            this.Type = type;
        }

        public virtual void Draw(SpriteBatch batch, Vector2 position) {
            var atlas = Registry.TextureItems;
            var texCoord = this.Type.TextureCoord;
            batch.Draw(
                atlas.Texture,
                position,
                atlas.GetRegion(texCoord.X, texCoord.Y),
                Color.White);
        }

    }

    public class ItemType {

        public readonly Point TextureCoord;

        public ItemType(Point textureCoord) {
            this.TextureCoord = textureCoord;
        }

        public virtual Item Instance() {
            return new Item(this);
        }

    }
}