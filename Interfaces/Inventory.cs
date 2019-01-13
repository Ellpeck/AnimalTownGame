using AnimalTownGame.Interfaces.Components;
using AnimalTownGame.Main;
using AnimalTownGame.Objects.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Interfaces {
    public class Inventory : ItemInterface {

        private static readonly Texture2D Texture = GameImpl.LoadContent<Texture2D>("Interfaces/Inventory");
        private readonly Player player;
        private Vector2 position;
        private InvContextMenu contextMenu;

        public Inventory(Player player) {
            this.player = player;
        }

        public override bool OnMouse(MouseButton button, PressType type) {
            if (type == PressType.Pressed) {
                var moused = this.GetMousedComponent();
                if (this.contextMenu != null && moused != this.contextMenu) {
                    this.Components.Remove(this.contextMenu);
                    this.contextMenu = null;
                }
                if (button == MouseButton.Right) {
                    var slot = moused as ItemSlot;
                    if (slot != null) {
                        this.contextMenu = new InvContextMenu(this, slot);
                        this.AddComponent(this.contextMenu);
                        return true;
                    }
                }
            }
            return base.OnMouse(button, type);
        }

        public override void Draw(SpriteBatch batch) {
            batch.Draw(Texture, this.position, Color.White);
            base.Draw(batch);
        }

        public override void Init(Viewport viewport, Size2 viewportSize) {
            base.Init(viewport, viewportSize);
            this.position = (Vector2) viewportSize / 2 - new Vector2(Texture.Width, Texture.Height) / 2;
            for (var y = 0; y < 4; y++)
                for (var x = 0; x < 6; x++)
                    this.AddSlot(new ItemSlot(this,
                        this.position + new Vector2(5 + x * 16 + (y % 2 == 0 ? 0 : 8), 4 + y * 14),
                        this.player.Inventory, 6 * y + x));
        }

    }
}