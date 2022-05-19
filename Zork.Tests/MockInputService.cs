using System;

namespace Zork.Tests
{
    class MockInputService : IInputService
    { 
        public event EventHandler<string> InputReceived;

        public void ProcessInput(string inputString)
        {
            InputReceived?.Invoke(this, inputString);
        }
    }
}
