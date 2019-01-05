using System.Collections.Generic;
using AnimalTownGame.Maps;
using AnimalTownGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnimalTownGame.Main {
    public class GameImpl : Game {

        public static GameImpl Instance { get; private set; }

        private readonly GraphicsDeviceManager graphicsManager;
        public SpriteBatch SpriteBatch { get; private set; }
        private Camera camera;

        private Dictionary<string, Map> maps = new Dictionary<string, Map>();
        public Map CurrentMap { get; private set; }

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

            this.CurrentMap = this.AddMap(new Map("Town", 64, 64));
            for (var x = 0; x < 64; x++)
                for (var y = 0; y < 64; y++)
                    if (x % 3 == 0 && y % 2 == 0)
                        this.CurrentMap[x, y] = Registry.TilePath.Instance(new Point(x, y));
                    else
                        this.CurrentMap[x, y] = Registry.TileGrass.Instance(new Point(x, y));

            this.camera = new Camera(null) {Scale = 80F};
            this.camera.FixPosition(this.CurrentMap);
            this.camera.PanToPosition = new Vector2(20, 20);
            this.camera.PanSpeed = 0.01F;
        }

        protected override void Update(GameTime gameTime) {
            this.camera.Update(this.CurrentMap);
        }

        protected override void Draw(GameTime gameTime) {
            MapRenderer.RenderMap(this.SpriteBatch, this.CurrentMap, this.GraphicsDevice.Viewport, this.camera);
        }

        private Map AddMap(Map map) {
            this.maps[map.Name] = map;
            return map;
        }

    }
}