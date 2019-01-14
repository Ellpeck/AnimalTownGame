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
        public DateTime LastPlayedTime;
        public PlayerInfo Player;
        public List<MapInfo> Maps;

        public SaveData(int seed, DateTime time, Player player, IEnumerable<Map> maps) {
            this.Seed = seed;
            this.LastPlayedTime = time;
            this.Player = new PlayerInfo(player);
            this.Maps = new List<MapInfo>();
            foreach (var map in maps) {
                if (map.CanHaveFurniture)
                    this.Maps.Add(new MapInfo(map));
            }
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
        public List<FurnitureInfo> Furniture;

        public MapInfo(Map map) {
            this.Name = map.Name;
            this.Furniture = new List<FurnitureInfo>();
            foreach (var obj in map.StaticObjects) {
                var furniture = obj as Furniture;
                if (furniture == null)
                    continue;
                this.Furniture.Add(new FurnitureInfo(furniture));
            }
        }

        public MapInfo() {
        }

        public void Load(Dictionary<string, Map> maps) {
            var map = maps[this.Name];
            foreach (var furniture in this.Furniture) {
                map.StaticObjects.Add(furniture.Load(map));
            }
        }

    }

    [Serializable]
    public class FurnitureInfo {

        public string Type;
        public Vector2 Position;

        public FurnitureInfo(Furniture furniture) {
            this.Type = furniture.Type.Name;
            this.Position = furniture.Position;
        }

        public FurnitureInfo() {
        }

        public Furniture Load(Map map) {
            var type = (FurnitureType) Registry.ItemTypes[this.Type];
            return new Furniture(type, map, this.Position);
        }

    }
}