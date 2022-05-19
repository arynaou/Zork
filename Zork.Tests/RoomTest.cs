using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zork.Tests
{
    [TestClass]
    public class RoomTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            { 
                Room room = new Room(_name, _description);
                Assert.AreEqual(_name, room.Name);
                Assert.AreEqual(_description, room.Description);
            }

            { 
                Room room = new Room(_name);    
                Assert.AreEqual(_name, room.Name);
                Assert.IsNull(room.Description);
            }
        }

        [TestMethod]
        public void TestFunction()
        {
            Room room = new Room(_name, _description);
            Assert.AreEqual(room.ToString(), _name);
        }

        string _name = "West of House";
        string _description = "A description";
    }
}
