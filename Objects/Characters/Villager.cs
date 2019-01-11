using System;
using System.Collections.Generic;
using AnimalTownGame.Maps;
using AnimalTownGame.Misc;
using Microsoft.Xna.Framework;

namespace AnimalTownGame.Objects.Characters {
    public class Villager : Character {

        private Stack<Point> path;
        private OnPathEnded onPathEnded;

        public Villager(string name, Map map, Vector2 position) : base(name, map, position) {
        }

        public void Pathfind(Point point, OnPathEnded callback) {
            this.path = PathFinding.FindPath(this.Map, this, this.Position.ToPoint(), point, 10000);
            this.onPathEnded = callback;
        }

        public override void Update(GameTime gameTime, bool isCurrent) {
            if (this.path == null && this.Map.Ticks % 120 == 0 && this.Map.Random.NextDouble() <= 0.4) {
                const int dist = 5;
                var point = new Point(this.Map.Random.Next(-dist, dist + 1), this.Map.Random.Next(-dist, dist + 1));
                this.Pathfind(this.Position.ToPoint() + point, null);
                this.WalkSpeed = 0.015F;
            }

            if (this.path != null) {
                while (true) {
                    var dest = this.path.Peek().ToVector2() + Vector2.One / 2F;
                    if (Vector2.Distance(this.Position, dest) <= this.WalkSpeed * 1.5) {
                        this.path.Pop();
                        if (this.path.Count <= 0) {
                            this.path = null;
                            if (this.onPathEnded != null)
                                this.onPathEnded();
                        } else
                            continue;
                    } else {
                        var move = dest - this.Position;
                        move.Normalize();
                        this.Velocity += move * this.WalkSpeed;
                    }
                    break;
                }
            }

            base.Update(gameTime, isCurrent);
        }

        public delegate void OnPathEnded();

    }
}