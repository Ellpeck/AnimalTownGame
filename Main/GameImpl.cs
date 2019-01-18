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
                PreferredBackBufferHeight = 720,
                SynchronizeWithVerticalRetrace = false
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

            var town = this.AddMap(MapGenerator.GenerateTown(this.MapSeed, data == null));

            var villager = new Villager("Player", town, new Vector2(35.5F, 32.5F));
            town.AddObject(villager);

            var houseMap = this.AddMap(MapGenerator.GenerateHouse("House1", new Point(20, 20)));
            var house = new VillagerHouse(0, town, new Vector2(20, 20), houseMap.Name);
            town.AddObject(house);

            if (data != null) {
                foreach (var map in data.Maps)
                    map.Load(this.Maps);
                this.Player = data.Player.Load(this.Maps);

                this.lastRealTimeUpdate = data.LastRealTimeUpdate;
                this.ForceRealTimeUpdate();
            } else {
                this.Player = new Player(town, new Vector2(22.5F, 22.5F));
                town.AddObject(this.Player);
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

            if ((CurrentTime - this.lastRealTimeUpdate).Minutes >= 10)
                this.ForceRealTimeUpdate();

            this.Camera.Update(this.CurrentMap);
        }

        private void ForceRealTimeUpdate() {
            var passed = CurrentTime - this.lastRealTimeUpdate;
            Console.WriteLine("Update at " + CurrentTime + ", last at " + this.lastRealTimeUpdate + " (" + passed + " passed)");
            foreach (var map in this.Maps.Values)
                map.UpdateRealTime(CurrentTime, this.lastRealTimeUpdate, passed);
            this.lastRealTimeUpdate = CurrentTime;
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
            SaveManager.Save("save", new SaveData(this.MapSeed, this.lastRealTimeUpdate, this.Player, this.Maps.Values));
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