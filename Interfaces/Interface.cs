using System.Collections.Generic;
using AnimalTownGame.Main;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Interfaces {
    public class Interface {

        public readonly List<InterfaceComponent> Components = new List<InterfaceComponent>();

        public virtual void Update(GameTime time) {
            foreach (var comp in this.Components)
                comp.Update(time);
        }

        public virtual void Draw(SpriteBatch batch) {
            foreach (var comp in this.Components)
                comp.Draw(batch);
        }

        public virtual void InitPositions(Viewport viewport, Size2 viewportSize) {
        }

        public virtual void OnOpen() {
        }

        public virtual void OnClose() {
        }

    }
}