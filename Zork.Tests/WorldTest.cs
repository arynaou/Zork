using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;

namespace Zork.Tests
{
    [TestClass]
    public class WorldTest
    {
        [TestMethod]
        public void TestDeserialize()
        {
            const string expectedRoomName = "West of House";

            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText("GameTest.json"));
            Assert.IsNotNull(game.World);
            Assert.AreEqual(game.StartingLocation, expectedRoomName);

        }
    }
}
