using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AnimalTownGame.Main {
    public static class CutsceneManager {

        private static float fadeSpeed;
        private static float fadePercentage;
        private static OnFaded fadeCallback;
        public static bool IsCutsceneActive => fadePercentage > 0F;

        public static void Update() {
            if (fadeSpeed != 0) {
                fadePercentage += fadeSpeed;
                if (fadeSpeed > 0 ? fadePercentage >= 1 : fadePercentage <= 0) {
                    fadeSpeed = 0;
                    if (fadeCallback != null)
                        fadeCallback();
                }
            }
        }

        public static void Draw(SpriteBatch batch, Viewport viewport) {
            if (fadePercentage > 0F) {
                batch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
                batch.FillRectangle(Vector2.Zero, new Size2(viewport.Width, viewport.Height), Color.Black * fadePercentage);
                batch.End();
            }
        }

        public static void Fade(float speed, OnFaded callback = null) {
            fadeSpeed = speed;
            fadeCallback = callback;
        }

        public delegate void OnFaded();

    }
}