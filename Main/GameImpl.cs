using Microsoft.Xna.Framework;

namespace AnimalTownGame.Main {
    public class GameImpl : Game {

        public static GameImpl Instance { get; private set; }

        private readonly GraphicsDeviceManager graphicsManager;

        public GameImpl() {
            Instance = this;

            this.graphicsManager = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };
            this.Content.RootDirectory = "Content";
            this.Window.AllowUserResizing = true;
        }

    }
}