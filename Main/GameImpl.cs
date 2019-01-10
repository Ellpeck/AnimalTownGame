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
        private Camera camera;

        private Dictionary<string, Map> maps = new Dictionary<string, Map>();
        public Player Player { get; private set; }
        public Map CurrentMap => this.Player.Map;

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

            var house = new VillagerHouse(VillagerHouse.Textures[0], town, new Vector2(20, 20));
            town.StaticObjects.Add(house);

            this.camera = new Camera(this.Player) {Scale = 80F};
            this.camera.FixPosition(this.CurrentMap);

            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                    if (x % 2 == 0)
                        town.SetTile(new Point(40 + x, 40 + y), Registry.TileWater);
        }

        protected override void Update(GameTime gameTime) {
            InputManager.Update();

            foreach (var map in this.maps.Values)
                map.Update(gameTime, map == this.CurrentMap);

            var now = DateTime.Now;
            var passed = now.Subtract(this.lastRealTimeUpdate);
            if (passed.Minutes >= 10) {
                foreach (var map in this.maps.Values)
                    map.UpdateRealTime(now, this.lastRealTimeUpdate, passed);
                this.lastRealTimeUpdate = now;
            }

            this.camera.Update(this.CurrentMap);
        }

        protected override void Draw(GameTime gameTime) {
            this.GraphicsDevice.Clear(Color.Aqua);
            MapRenderer.RenderMap(this.SpriteBatch, this.CurrentMap, this.GraphicsDevice.Viewport, this.camera);
        }

        private Map AddMap(Map map) {
            this.maps[map.Name] = map;
            return map;
        }

        public static T LoadContent<T>(string name) {
            return Instance.Content.Load<T>(name);
        }

    }
}