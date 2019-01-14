using System;
using System.Collections.Generic;
using AnimalTownGame.Maps;
using AnimalTownGame.Objects;
using AnimalTownGame.Objects.Characters;
using AnimalTownGame.Objects.Static;
using AnimalTownGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Main {
    public class GameImpl : Game {

        public static GameImpl Instance { get; private set; }

        private readonly GraphicsDeviceManager graphicsManager;
        public SpriteBatch SpriteBatch { get; private set; }
        public Camera Camera { get; private set; }

        public readonly Dictionary<string, Map> Maps = new Dictionary<string, Map>();
        public Player Player { get; private set; }
        public Map CurrentMap => this.Player?.Map;
        public int MapSeed;

        public static DateTime CurrentTime => DateTime.Now;
        private DateTime lastRealTimeUpdate = CurrentTime;

        public GameImpl() {
            Instance = this;

            this.graphicsManager = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };
            this.Content.RootDirectory = "Content";
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += this.OnWindowSizeChange;
        }

        protected override void LoadContent() {
            new Registry();
            this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);

            var data = SaveManager.Load("save");
            this.MapSeed = data?.Seed ?? new Random().Next();

            var town = this.AddMap(MapGenerator.GenerateTown(this.MapSeed));

            var villager = new Villager("Player", town, new Vector2(35.5F, 32.5F));
            town.DynamicObjects.Add(villager);

            var houseMap = this.AddMap(MapGenerator.GenerateHouse("House1", new Point(20, 20)));
            var house = new VillagerHouse(0, town, new Vector2(20, 20), houseMap.Name);
            town.StaticObjects.Add(house);

            if (data != null) {
                foreach (var map in data.Maps)
                    map.Load(this.Maps);
                this.Player = data.Player.Load(this.Maps);

                var passed = CurrentTime.Subtract(data.LastPlayedTime);
                foreach (var map in this.Maps.Values)
                    map.UpdateRealTime(CurrentTime, data.LastPlayedTime, passed);
            } else {
                this.Player = new Player(town, new Vector2(22.5F, 22.5F));
                town.DynamicObjects.Add(this.Player);
            }

            this.Camera = new Camera(this.Player);
            this.Camera.FixPosition(this.CurrentMap);
        }

        protected override void Update(GameTime gameTime) {
            InterfaceManager.Update(gameTime);
            InputManager.Update(this.CurrentMap, this.Camera);
            CutsceneManager.Update();

            foreach (var map in this.Maps.Values)
                map.Update(gameTime, map == this.CurrentMap);

            var passed = CurrentTime.Subtract(this.lastRealTimeUpdate);
            if (passed.Minutes >= 10) {
                foreach (var map in this.Maps.Values)
                    map.UpdateRealTime(CurrentTime, this.lastRealTimeUpdate, passed);
                this.lastRealTimeUpdate = CurrentTime;
            }

            this.Camera.Update(this.CurrentMap);
        }

        protected override void Draw(GameTime gameTime) {
            var view = this.GraphicsDevice.Viewport;
            if (this.CurrentMap != null) {
                this.GraphicsDevice.Clear(this.CurrentMap.IsInside ? new Color(36, 39, 51) : Color.Aqua);
                MapRenderer.RenderMap(this.SpriteBatch, this.CurrentMap, view, this.Camera);
            } else
                this.GraphicsDevice.Clear(Color.Black);

            InterfaceManager.Draw(this.SpriteBatch, view, this.Camera);
            CutsceneManager.Draw(this.SpriteBatch, view);
        }

        protected override void UnloadContent() {
            SaveManager.Save("save", new SaveData(this.MapSeed, CurrentTime, this.Player, this.Maps.Values));
        }

        private void OnWindowSizeChange(object window, EventArgs args) {
            this.Camera.FixPosition(this.CurrentMap);
            InterfaceManager.OnWindowSizeChange(this.GraphicsDevice.Viewport);
        }

        private Map AddMap(Map map) {
            this.Maps[map.Name] = map;
            return map;
        }

        public static T LoadContent<T>(string name) {
            return Instance.Content.Load<T>(name);
        }

    }
}