namespace Zork
{
    public interface IOutputService
    {
        void Write(object value);

        void WriteLine(object value);

        void MoveOutput(int value);

        void ScoreOutput(int value);

        void LocationOutput(string value);

        void QuitGame();

        void BackgroundOutput(Player player);

        void ItemOutput(Player player);

        void EnemyOutput(Player player, float hitpoint, float maxHitpoint);

        void AudioOutput(string state);
    }
}
