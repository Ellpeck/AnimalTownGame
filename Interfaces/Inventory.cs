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
        public InvContextMenu ContextMenu;

        public Inventory(Player player) {
            this.player = player;
        }

        public override bool OnMouse(MouseButton button, PressType type) {
            if (type == PressType.Pressed) {
                var moused = this.GetMousedComponent();
                if (this.ContextMenu != null && moused != this.ContextMenu)
                    this.ContextMenu.Close();
                if (button == MouseButton.Right) {
                    var slot = moused as ItemSlot;
                    if (slot?.Items[slot.Index] != null) {
                        this.ContextMenu = new InvContextMenu(this, slot, MousePos);
                        this.AddComponent(this.ContextMenu);
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