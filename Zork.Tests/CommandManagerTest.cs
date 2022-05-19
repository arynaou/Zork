using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System;

namespace Zork.Tests
{
    [TestClass]
    public class CommandManagerTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            commandManager = new CommandManager();
            Assert.IsNotNull(commandManager);
        }

        [TestMethod]
        public void TestFunction()
        {
            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText("GameTest.json"));
            { 
                commandManager = new CommandManager();
                Assert.IsFalse(commandManager.PerformCommand(game, "QUIT"));
            }

            {
                Command[] commands =
                {
                    new Command("QUIT", new string[] { "QUIT", "Q" }, (game, CommandContext) => Console.WriteLine("Quit Game")),
                };
                
                commandManager = new CommandManager(commands);
                Assert.IsNotNull(commandManager.PerformCommand(game, "QUIT"));
            }
        }

        CommandManager commandManager;
    }
}
