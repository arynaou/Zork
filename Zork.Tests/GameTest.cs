using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System;

namespace Zork.Tests
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            { 
                Game newGame = new Game();
                Assert.IsNotNull(newGame);
            }

            {
                World world = JsonConvert.DeserializeObject<Game>(File.ReadAllText("GameTest.json")).World;
                Player player = new Player(world, "West of House");
                game = new Game(world, player, outputService, inputService);
                Assert.AreEqual(game.World, world);
                Assert.AreEqual(game.Player, player);
            }
        }

        [TestMethod]
        public void TestFunction()
        {
            string roomsFilename = "GameTest.json";
            {
                game = Game.StartFromFile(roomsFilename, outputService, inputService);
            }

            {
                Assert.ThrowsException<FileNotFoundException>(() => { Game.StartFromFile(".txt", outputService, inputService); });
            }

            {
                outputService = null;
                Assert.ThrowsException<ArgumentNullException>(() => { Game.StartFromFile(roomsFilename, outputService, inputService); });
            }

            {
                inputService = null;
                Assert.ThrowsException<ArgumentNullException>(() => { Game.StartFromFile(roomsFilename, outputService, inputService); });
            }
        }

        [TestMethod]
        public void TestEvent()
        {
            OutputServiceTest outputService = new OutputServiceTest();
            MockInputService inputService = new MockInputService();
            Game newGame = Game.StartFromFile("GameTest.json", outputService, inputService);

            {
                Room westOfHouse = newGame.World.RoomsByName["West of House"];
                bool locationChangedFired = false;
                newGame.Player.LocationChanged += (sender, args) => { locationChangedFired = true; };

                newGame.Player.Move(Directions.SOUTH);
                Assert.IsTrue(locationChangedFired);

                Room southOfHouse = newGame.World.RoomsByName["South of House"];
                Assert.AreEqual(southOfHouse, newGame.Player.Location);
            }

            {
                bool scoreChangedFired = false;
                newGame.Player.ScoreChanged += (sender, args) => { scoreChangedFired = true;  };

                newGame.Player.Score++;
                Assert.IsTrue(scoreChangedFired);
            }

            {
                Assert.IsTrue(newGame.IsRunning);
                bool isRunningFired = false;
                newGame.IsRunningChanged += (sender, args) => { isRunningFired = true; };
                newGame.IsRunning = false;
                Assert.IsTrue(isRunningFired);
            }

            {
                bool isInputInvoked = false;
                newGame.InputService.InputReceived += (sender, args) => { isInputInvoked = true; };
                inputService.ProcessInput("LOOK");
                inputService.ProcessInput("HI");
                Assert.IsTrue(isInputInvoked);
            }
        }

        [TestMethod]
        public void TestCommand()
        {
            OutputServiceTest outputService = new OutputServiceTest();
            MockInputService inputService = new MockInputService();
            Game newGame = Game.StartFromFile("GameTest.json", outputService, inputService);

            Assert.IsNotNull(newGame.CommandManager.PerformCommand(newGame, "LOOK"));
            Assert.IsNotNull(newGame.CommandManager.PerformCommand(newGame, "QUIT"));
            Assert.IsNotNull(newGame.CommandManager.PerformCommand(newGame, "SCORE"));
            Assert.IsNotNull(newGame.CommandManager.PerformCommand(newGame, "REWARD"));
            Assert.IsNotNull(newGame.CommandManager.PerformCommand(newGame, "EAST"));
            Assert.IsNotNull(newGame.CommandManager.PerformCommand(newGame, "WEST"));
            Assert.IsNotNull(newGame.CommandManager.PerformCommand(newGame, "SOUTH"));
            Assert.IsNotNull(newGame.CommandManager.PerformCommand(newGame, "NORTH"));
            Assert.IsNotNull(newGame.CommandManager.PerformCommand(newGame, "UP"));
            Assert.IsNotNull(newGame.CommandManager.PerformCommand(newGame, "DOWN"));
        }

        static OutputServiceTest outputService = new OutputServiceTest();
        static MockInputService inputService = new MockInputService();
        Game game;
    }
}
