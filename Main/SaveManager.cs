using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using AnimalTownGame.Items;
using AnimalTownGame.Maps;
using AnimalTownGame.Objects.Characters;
using AnimalTownGame.Objects.Static;
using Microsoft.Xna.Framework;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace AnimalTownGame.Main {
    public static class SaveManager {

        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(SaveData));

        public static void Save(string name, SaveData data) {
            var file = GetLocation(name);
            var info = new FileInfo(file);
            if (!info.Directory.Exists)
                info.Directory.Create();
            else if (File.Exists(file))
                File.Delete(file);
            using (var stream = File.Create(file))
                Serializer.Serialize(stream, data);
        }

        public static SaveData Load(string name) {
            var file = GetLocation(name);
            if (File.Exists(file))
                using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
                    return (SaveData) Serializer.Deserialize(stream);
            return null;
        }

        private static string GetLocation(string name) {
            var appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appdata, "AnimalTownGame", "Save", name + ".sav");
        }

    }

    [Serializable]
    public class SaveData {

        public int Seed;
        public DateTime LastRealTimeUpdate;
        public PlayerInfo Player;
        public List<MapInfo> Maps;

        public SaveData(int seed, DateTime lastRealTimeUpdate, Player player, IEnumerable<Map> maps) {
            this.Seed = seed;
            this.LastRealTimeUpdate = lastRealTimeUpdate;
            this.Player = new PlayerInfo(player);
            this.Maps = new List<MapInfo>();
            foreach (var map in maps)
                this.Maps.Add(new MapInfo(map));
        }

        public SaveData() {
        }

    }

    [Serializable]
    public class PlayerInfo {

        public string Map;
        public Vector2 Position;
        public List<string> Items;

        public PlayerInfo(Player player) {
            this.Map = player.Map.Name;
            this.Position = player.Position;
            this.Items = new List<string>();
            foreach (var item in player.Inventory) {
                this.Items.Add(item == null ? null : item.Type.Name);
            }
        }

        public PlayerInfo() {
        }

        public Player Load(Dictionary<string, Map> maps) {
            var player = new Player(maps[this.Map], this.Position);
            for (var i = 0; i < this.Items.Count; i++) {
                var item = this.Items[i];
                if (item == null)
                    player.Inventory[i] = null;
                else
                    player.Inventory[i] = Registry.ItemTypes[item].Instance();
            }
            player.Map.DynamicObjects.Add(player);
            return player;
        }

    }

    [Serializable]
    public class MapInfo {

        public string Name;
        public List<StaticObjectInfo> StaticObjects;

        public MapInfo(Map map) {
            this.Name = map.Name;
            this.StaticObjects = new List<StaticObjectInfo>();
            foreach (var obj in map.StaticObjects) {
                var furniture = obj as Furniture;
                if (furniture != null) {
                    this.StaticObjects.Add(new FurnitureInfo(furniture));
                    continue;
                }
                var tree = obj as FruitTree;
                if (tree != null) {
                    this.StaticObjects.Add(new FruitTreeInfo(tree));
                }
            }
        }

        public MapInfo() {
        }

        public void Load(Dictionary<string, Map> maps) {
            var map = maps[this.Name];
            foreach (var obj in this.StaticObjects)
                map.StaticObjects.Add(obj.Load(map));
        }

    }

    [Serializable]
    [XmlInclude(typeof(FurnitureInfo))]
    [XmlInclude(typeof(FruitTreeInfo))]
    public abstract class StaticObjectInfo {

        public Vector2 Position;

        public StaticObjectInfo(Vector2 position) {
            this.Position = position;
        }

        public StaticObjectInfo() {
        }

        public abstract StaticObject Load(Map map);

    }

    [Serializable]
    public class FurnitureInfo : StaticObjectInfo {

        public string Type;

        public FurnitureInfo(Furniture furniture) : base(furniture.Position) {
            this.Type = furniture.Type.Name;
        }

        public FurnitureInfo() {
        }

        public override StaticObject Load(Map map) {
            var type = (FurnitureType) Registry.ItemTypes[this.Type];
            return new Furniture(type, map, this.Position);
        }

    }

    [Serializable]
    public class FruitTreeInfo : StaticObjectInfo {

        public string Type;
        public long FruitTime;

        public FruitTreeInfo(FruitTree tree) : base(tree.Position) {
            this.Type = tree.Type.Name;
            this.FruitTime = tree.FruitTimer.Ticks;
        }

        public FruitTreeInfo() {
        }

        public override StaticObject Load(Map map) {
            foreach (var type in FruitTree.Types)
                if (type.Name == this.Type) {
                    return new FruitTree(type, map, this.Position) {
                        FruitTimer = TimeSpan.FromTicks(this.FruitTime)
                    };
                }
            return null;
        }

    }
}