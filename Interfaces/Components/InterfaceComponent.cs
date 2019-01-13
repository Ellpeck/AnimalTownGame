using System;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Interfaces.Components {
    public class InterfaceComponent : Interface, IComparable<InterfaceComponent> {

        public readonly Interface Interface;
        public Vector2 Position;

        public InterfaceComponent(Interface iface, Vector2 position) {
            this.Interface = iface;
            this.Position = position;
        }

        public virtual int GetPriority() {
            return 0;
        }

        public int CompareTo(InterfaceComponent other) {
            return other.GetPriority().CompareTo(this.GetPriority());
        }

        public virtual bool IsMouseOver() {
            return false;
        }

        public bool IsMousedComponent() {
            return this.Interface.GetMousedComponent() == this;
        }

    }
}