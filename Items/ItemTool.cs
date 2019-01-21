using Microsoft.Xna.Framework;

namespace AnimalTownGame.Items {
    public class ItemTool : Item {

        private readonly ToolType type;

        public ItemTool(ToolType type) : base(type) {
            this.type = type;
        }

    }

    public class ToolType : ItemType {

        public ToolType(string name, Point textureCoord) : base(name, textureCoord) {
        }

        public override Item Instance() {
            return new ItemTool(this);
        }

    }
}