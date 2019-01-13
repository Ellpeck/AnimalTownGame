using System;
using AnimalTownGame.Main;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimalTownGame.Interfaces.Components {
    public class ComponentHover : InterfaceComponent {

        private readonly string text;

        public ComponentHover(Interface iface, string text) : base(iface, Vector2.Zero) {
            this.text = text;
        }

        public override void Draw(SpriteBatch batch) {
            base.Draw(batch);
            batch.DrawInfoBox(InterfaceManager.NormalFont, this.text, MousePos + new Vector2(4, 3), 0.2F);
        }

        public override int GetPriority() {
            return 10;
        }

    }
}