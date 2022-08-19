namespace GameCore
{
    public class GameInfo
    {
        public State CurrentState { get; set; } = State.Start;
        public Scene CurrentScene { get; set; } = Scene.MainMenu;
        public int CurrentLevelIndex { get; set; } // Starts with 0.

        public int BestScore { get; set; }

        public enum State
        {
            Start,
            Play,
            Paused,
            Over,
            Success
        }

        public enum Scene
        {
            MainMenu = 0,
            Game = 1
        }
    }
}