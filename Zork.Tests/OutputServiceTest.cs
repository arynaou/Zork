using System;

namespace Zork.Tests
{
    class OutputServiceTest : IOutputService
    {
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

        public void Write(object value)
        {
            Console.Write(value);
        }

        public void WriteLine(object value)
        {
            Console.WriteLine(value);
        }
    }
}
