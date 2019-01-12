using AnimalTownGame.Main;
using AnimalTownGame.Objects.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Interfaces {
    public class Inventory : Interface {

        private static readonly Texture2D Texture = GameImpl.LoadContent<Texture2D>("Interfaces/Inventory");
        private readonly Player player;
        private readonly Vector2[] slotOffsets;
        private Size2 slotScale;
        private Vector2 position;

        public Inventory(Player player) {
            this.player = player;
            this.slotOffsets = new Vector2[this.player.Inventory.Length];
        }

        public override void Draw(SpriteBatch batch) {
            batch.Draw(Texture, this.position, Color.White);
            for (var i = 0; i < this.slotOffsets.Length; i++) {
                var slot = this.player.Inventory[i];
                if (slot != null)
                    slot.Draw(batch, this.position + this.slotOffsets[i]);
            }
        }

        public override void InitPositions(Viewport viewport, Size2 viewportSize) {
            this.position = (Vector2) viewportSize / 2 - new Vector2(Texture.Width, Texture.Height) / 2;
            for (var x = 0; x < 6; x++)
                for (var y = 0; y < 4; y++) {
                    this.slotOffsets[6 * y + x] = new Vector2(5 + x * 16 + (y % 2 == 0 ? 0 : 8), 4 + y * 14);
                }

            this.slotScale = new Size2(16, 16);
        }

    }
}