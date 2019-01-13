using AnimalTownGame.Items;
using AnimalTownGame.Maps;
using AnimalTownGame.Rendering;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AnimalTownGame.Main {
    public class Registry {

        public static readonly TextureAtlas TextureOutside = new TextureAtlas("Tilesets/Outside", 16, 16);
        public static readonly TextureAtlas TextureInside = new TextureAtlas("Tilesets/Inside", 16, 16);
        public static readonly TextureAtlas TextureItems = new TextureAtlas("Items/Icons", 16, 16);

        public static readonly TileType TileGrass = new TileType(new Point(0, 0));
        public static readonly TileType TilePath = new AutoTextureType(new Point(1, 0), false).SetWalkability(200);
        public static readonly TileType TileWater = new AutoTextureType(new Point(8, 3), true).SetWalkability(int.MaxValue);
        public static readonly TileType TileDarkGrass = new AutoTextureType(new Point(4, 0), true).SetWalkability(1500);

        public static readonly TileType TileFloor = new TileType(new Point(0, 0), TextureInside);
        public static readonly TileType TileWallTrim = new WallTrimType(new Point(1, 0), TextureInside).SetWalkability(int.MaxValue);
        public static readonly TileType TileWall = new TileType(new Point(0, 1), TextureInside).SetWalkability(int.MaxValue);

        public static readonly ItemType ItemWhiteLamp = new FurnitureType("White Lamp", "WhiteLamp", new RectangleF(-0.5F, -1.5F, 1, 2), new RectangleF(-0.25F, -0.2F, 0.5F, 0.6F));
        public static readonly ItemType ItemRedRug = new FurnitureType("Red Rug", "RedRug", new RectangleF(-1.5F, -0.5F, 3, 2), RectangleF.Empty, new RectangleF(-1.5F, -0.5F, 3, 2)).SetDepthOffset(-1F);
        public static readonly ItemType ItemWoodDiningTable = new FurnitureType("Wood Dining Table", "WoodDiningTable", new RectangleF(-0.5F, -0.5F, 2, 2), new RectangleF(-0.5F, 0.25F, 2, 1.25F));
        public static readonly ItemType ItemMarbleCounter = new FurnitureType("Marble Counter", "MarbleCounter", new RectangleF(-0.5F, -1.5F, 1, 2), new RectangleF(-0.5F, -0.5F, 1, 1));
        public static readonly ItemType ItemMarbleCounterSink = new FurnitureType("Marble Counter with Sink", "MarbleCounterSink", new RectangleF(-0.5F, -1.5F, 1, 2), new RectangleF(-0.5F, -0.5F, 1, 1));
        public static readonly ItemType ItemStove = new FurnitureType("Stove", "Stove", new RectangleF(-0.5F, -1.5F, 1, 2), new RectangleF(-0.5F, -0.5F, 1, 1));

    }
}