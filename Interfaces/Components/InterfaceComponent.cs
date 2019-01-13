using System;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Interfaces.Components {
    public class InterfaceComponent : Interface, IComparable<InterfaceComponent> {

        public readonly Interface Interface;

        public InterfaceComponent(Interface iface) {
            this.Interface = iface;
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
            return this.IsMouseOver() && this.Interface.GetMousedComponent() == this;
        }

    }
}