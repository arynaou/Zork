using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;

namespace Zork.Tests
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void Deserialize()
        {
            Assert.IsNotNull(game);
        }

        [TestMethod]
        public void TestFunction()
        {
            Assert.IsNotNull(player.Move(Directions.NORTH));
        }

        [TestMethod]
        public void TestInstantiate()
        {
            Room room = new Room("West of House","This is a room");
            player.PreviousRoom = room;
            Assert.AreEqual(player.PreviousRoom,room);
        }

        [TestMethod]
        public void TestScore()
        {
            player.Score++;
            Assert.IsNotNull(player.Score);
        }

        static Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText("GameTest.json"));
        static Player player = new Player(game.World, "West of House");
    }
}
