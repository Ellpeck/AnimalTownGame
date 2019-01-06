using System;
using System.Collections.Generic;
using AnimalTownGame.Maps;
using AnimalTownGame.Maps.Objects;
using AnimalTownGame.Maps.Objects.Static;
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

            var rand = new Random();
            var town = this.AddMap(new Map("Town", 64, 64));
            for (var x = 0; x < 64; x++)
                for (var y = 0; y < 64; y++)
                    town.SetTile(new Point(x, y), rand.NextDouble() >= 0.25F ? Registry.TileWater : Registry.TileGrass);
            town.SetTile(new Point(15, 15), Registry.TilePath);
            town.SetTile(new Point(10, 10), Registry.TilePath);

            var tree = new Tree(town, new Vector2(15.5F, 15.5F));
            town.StaticObjects.Add(tree);

            this.Player = new Player(town, new Vector2(10.5F, 10.5F));
            town.DynamicObjects.Add(this.Player);

            this.camera = new Camera(this.Player) {Scale = 80F};
            this.camera.FixPosition(this.CurrentMap);
        }

        protected override void Update(GameTime gameTime) {
            InputManager.Update();

            this.CurrentMap.Update(gameTime);

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
            this.GraphicsDevice.Clear(Color.Azure);
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