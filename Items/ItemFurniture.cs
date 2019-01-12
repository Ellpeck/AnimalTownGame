using Microsoft.Xna.Framework;

namespace AnimalTownGame.Items {
    public class ItemFurniture : Item {

        public ItemFurniture(ItemType type) : base(type) {
        }

    }

    public class FurnitureType : ItemType {

        public FurnitureType() : base(new Point(0, 0)) {
        }

        public override Item Instance() {
            return new ItemFurniture(this);
        }

    }
}