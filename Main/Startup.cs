namespace AnimalTownGame.Main {
    public static class Startup {

        public static void Main() {
            using (var game = new GameImpl()) {
                game.Run();
            }
        }
    }
}