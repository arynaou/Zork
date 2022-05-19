using System;

namespace Zork
{
    class ConsoleOutputService : IOutputService
    {
        public void Write(object value)
        {
            Console.Write(value);
        }

        public void WriteLine(object value)
        {
            Console.WriteLine(value);
        }

        public void AudioOutput(string state)
        {
        }

        public void BackgroundOutput(Player player)
        {
        }

        public void EnemyOutput(Player player, float hitpoint, float maxHitpoint)
        {
        }

        public void ItemOutput(Player player)
        {
        }

        public void LocationOutput(string value)
        {
        }

        public void MoveOutput(int value)
        {
        }

        public void QuitGame()
        {
        }

        public void ScoreOutput(int value)
        {
        }
    }
}
