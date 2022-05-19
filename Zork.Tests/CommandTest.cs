using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Zork.Tests
{
    [TestClass]
    public class CommandTest
    {
        [TestMethod]
        public void Equals()
        {
            {
                Command theLastCommand = command;

                Assert.IsTrue(command==anotherCommand);
                Assert.IsTrue(command == theLastCommand);
                Assert.IsTrue(command!=theOtherCommand);
            }

            {
                Assert.IsTrue(command.Equals(anotherCommand));
                Assert.IsFalse(command.Equals(theOtherCommand));
            }

            {
                Command nullCommand = null;
                Assert.IsFalse(nullCommand == anotherCommand);
            }

            {
                object commandObj = anotherCommand;
                Assert.IsTrue(command.Equals(commandObj));
                commandObj = theOtherCommand;
                Assert.IsFalse(command.Equals(commandObj));
                commandObj = new string("Test");
                Assert.IsFalse(command.Equals(commandObj));
            }
        }

        [TestMethod]
        public void NameTest()
        {
            Assert.AreEqual(command.ToString(), command.Name);
        }

        Command command = new Command("NORTH", "NORTH", (game, CommandContext) => Console.WriteLine("Move North"));
        Command anotherCommand = new Command("NORTH", new string[] { "NORTH", "N" }, (game, CommandContext) => Console.WriteLine("the other Move North"));
        Command theOtherCommand = new Command("SOUTH", new string[] { "SOUTH", "S" }, (game, CommandContext) => Console.WriteLine("Move South"));
    }
}
