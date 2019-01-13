using System.Collections.Generic;
using AnimalTownGame.Interfaces.Components;
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

        public virtual void Draw(SpriteBatch batch, Vector2 position, float scale) {
            var atlas = Registry.TextureItems;
            var texCoord = this.Type.TextureCoord;
            batch.Draw(
                atlas.Texture,
                position,
                atlas.GetRegion(texCoord.X, texCoord.Y),
                Color.White);
        }

        public virtual string GetName() {
            return this.Type.Name;
        }

        public virtual IEnumerable<InterfaceComponent> GetContextMenu(ItemSlot slot, InvContextMenu menu) {
            return null;
        }

    }

    public class ItemType {

        public readonly Point TextureCoord;
        public readonly string Name;

        public ItemType(string name, Point textureCoord) {
            this.TextureCoord = textureCoord;
            this.Name = name;
        }

        public virtual Item Instance() {
            return new Item(this);
        }

    }
}