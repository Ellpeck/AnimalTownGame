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

        private DateTime lastRealTimeUpdate = DateTime.Now;

        public GameImpl() {
            Instance = this;

            this.graphicsManager = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };
            this.Content.RootDirectory = "Content";
            this.Window.AllowUserResizing = true;
        }

        protected override void LoadContent() {
            new Registry();
            this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);

            var town = this.AddMap(MapGenerator.GenerateTown());
            this.Player = new Player(town, new Vector2(32.5F, 32.5F));
            town.DynamicObjects.Add(this.Player);

            var villager = new Villager("Player", town, new Vector2(35.5F, 32.5F));
            town.DynamicObjects.Add(villager);

            var houseMap = this.AddMap(MapGenerator.GenerateHouse("House1", new Point(20, 20)));
            var house = new VillagerHouse(VillagerHouse.Textures[0], town, new Vector2(20, 20), houseMap.Name, new Vector2(5.5F, 9.5F));
            town.StaticObjects.Add(house);

            this.Camera = new Camera(this.Player) {Scale = 80F};
            this.Camera.FixPosition(this.CurrentMap);
        }

        protected override void Update(GameTime gameTime) {
            InputManager.Update(this.CurrentMap, this.Camera);
            CutsceneManager.Update();

            foreach (var map in this.Maps.Values)
                map.Update(gameTime, map == this.CurrentMap);

            var now = DateTime.Now;
            var passed = now.Subtract(this.lastRealTimeUpdate);
            if (passed.Minutes >= 10) {
                foreach (var map in this.Maps.Values)
                    map.UpdateRealTime(now, this.lastRealTimeUpdate, passed);
                this.lastRealTimeUpdate = now;
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

            InputManager.Draw(this.SpriteBatch);
            CutsceneManager.Draw(this.SpriteBatch, view);
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