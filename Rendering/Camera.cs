using AnimalTownGame.Main;
using AnimalTownGame.Maps;
using AnimalTownGame.Objects;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Rendering {
    public class Camera {

        public Vector2 Position;
        public float Scale;
        public MapObject FollowedObject;

        public Vector2? PanToPosition;
        public OnPositionReached PanCallback;
        public float PanSpeed;

        public Camera(MapObject followedObject) {
            this.FollowedObject = followedObject;
        }

        public Matrix ViewMatrix => Matrix.CreateScale(this.Scale) * Matrix.CreateTranslation(-(int) this.Position.X, -(int) this.Position.Y, 0F);

        public void Update(Map map) {
            var desired = this.GetDesiredPosition(map);
            if (this.PanToPosition != null && this.PanSpeed > 0) {
                if (Vector2.Distance(this.Position, desired) <= this.PanSpeed * this.Scale) {
                    this.PanToPosition = null;
                    this.PanSpeed = 0;
                    if (this.PanCallback != null) {
                        this.PanCallback();
                        this.PanCallback = null;
                    }
                } else {
                    var diff = desired - this.Position;
                    diff.Normalize();
                    this.Position += diff * this.PanSpeed * this.Scale;
                }
            } else
                this.Position = Vector2.Lerp(this.Position, desired, 0.1F);
        }

        public void FixPosition(Map map) {
            this.Position = this.GetDesiredPosition(map);
        }

        private Vector2 GetDesiredPosition(Map map) {
            var viewport = GameImpl.Instance.GraphicsDevice.Viewport;

            Vector2 desired;
            if (this.PanToPosition != null)
                desired = this.PanToPosition.Value;
            else if (this.FollowedObject != null) {
                var bounds = this.FollowedObject.RenderBounds;
                desired = this.FollowedObject.Position + bounds.Position + new Vector2(bounds.Width / 2, bounds.Height / 3);
            } else
                return this.Position;
            desired -= new Vector2(viewport.Width / 2F, viewport.Height / 2F) / this.Scale;

            var maxX = map.WidthInTiles - viewport.Width / this.Scale;
            if (maxX < 0)
                desired.X = maxX / 2;
            else if (desired.X < 0)
                desired.X = 0;
            else if (desired.X > maxX)
                desired.X = maxX;

            var maxY = map.HeightInTiles - viewport.Height / this.Scale;
            if (maxY < 0)
                desired.Y = maxY / 2;
            else if (desired.Y < 0)
                desired.Y = 0;
            else if (desired.Y > maxY)
                desired.Y = maxY;

            return desired * this.Scale;
        }

        public Vector2 ToWorldPos(Vector2 pos) {
            return Vector2.Transform(pos, Matrix.Invert(this.ViewMatrix));
        }

        public Vector2 ToCameraPos(Vector2 pos) {
            return Vector2.Transform(pos, this.ViewMatrix);
        }

        public delegate void OnPositionReached();

    }
}