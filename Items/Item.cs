using System.Collections.Generic;
using System.Linq;
using AnimalTownGame.Interfaces.Components;
using AnimalTownGame.Main;
using AnimalTownGame.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Items {
    public class Item {

        public readonly ItemType Type;

        public Item(ItemType type) {
            this.Type = type;
        }

        public virtual void Draw(SpriteBatch batch, Vector2 position, float scale, float depth, bool border) {
            var atlas = Registry.TextureItems;
            if (border)
                batch.Draw(atlas.Texture, position, atlas.GetRegion(0, 1), Color.White, 0F, Vector2.Zero, scale, SpriteEffects.None, depth);

            var texCoord = this.Type.TextureCoord;
            batch.Draw(
                atlas.Texture,
                position,
                atlas.GetRegion(texCoord.X, texCoord.Y),
                Color.White, 0F, Vector2.Zero, scale, SpriteEffects.None, depth);
        }

        public virtual IEnumerable<ComponentButton> GetContextMenu(ItemSlot slot, InvContextMenu menu) {
            var game = GameImpl.Instance;
            yield return new ComponentButton(menu,
                new RectangleF(Vector2.Zero, new Size2(20, 6)), Locale.GetInterface("Drop"),
                (button, pressType) => {
                    if (button == MouseButton.Left && pressType == PressType.Pressed) {
                        var pos = ItemObject.GetFeasibleDropPos(game.CurrentMap, game.Player.Position);
                        if (pos == Vector2.Zero)
                            return false;
                        var item = new ItemObject(this, game.CurrentMap, pos);
                        game.CurrentMap.AddObject(item);
                        slot.Items[slot.Index] = null;
                        menu.Close();
                        return true;
                    }
                    return false;
                });
        }

    }

    public class ItemType {

        public readonly Point TextureCoord;
        public readonly string Name;

        public ItemType(string name, Point textureCoord) {
            this.TextureCoord = textureCoord;
            this.Name = name;
            Registry.ItemTypes[name] = this;
        }

        public virtual Item Instance() {
            return new Item(this);
        }
        
        public virtual string GetDisplayName() {
            return Locale.GetItem(this.Name);
        }
    }
}