using AnimalTownGame.Items;
using AnimalTownGame.Main;
using AnimalTownGame.Objects.Characters;
using AnimalTownGame.Objects.Static;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Interfaces {
    public class Storage : ItemInterface {

        private static readonly Texture2D Texture = GameImpl.LoadContent<Texture2D>("Interfaces/Storage");
        private readonly Furniture furniture;
        private Vector2 position;
        private Vector2 invTextOffset;
        private Vector2 storageTextOffset;

        public Storage(Player player, Furniture furniture) : base(player) {
            this.furniture = furniture;
        }

        public override void Draw(SpriteBatch batch) {
            batch.Draw(Texture, this.position, Color.White);

            batch.DrawString(InterfaceManager.NormalFont, Locale.GetInterface("Inventory"),
                this.position + this.invTextOffset, Color.White, 0F, Vector2.Zero, 0.2F, SpriteEffects.None, 0F);
            batch.DrawString(InterfaceManager.NormalFont, this.furniture.Type.GetDisplayName(),
                this.position + this.storageTextOffset, Color.White, 0F, Vector2.Zero, 0.2F, SpriteEffects.None, 0F);

            base.Draw(batch);
        }

        public override void Init(Viewport viewport, Size2 viewportSize) {
            base.Init(viewport, viewportSize);

            this.position = (Vector2) viewportSize / 2 - new Vector2(Texture.Width, Texture.Height) / 2;
            this.invTextOffset = new Vector2(5, 74);
            this.storageTextOffset = new Vector2(5, 4);

            this.AddSlotGrid(this.position + new Vector2(5, 12), 6, 4, this.furniture.Storage);
            this.AddSlotGrid(this.position + new Vector2(5, 82), 6, 4, this.Player.Inventory);
        }

    }
}