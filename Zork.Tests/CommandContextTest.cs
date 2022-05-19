using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Zork.Tests
{
    [TestClass]
    public class CommandContextTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            CommandContext commandContext = new CommandContext(commandString, command);
            Assert.AreEqual(commandContext.CommandString, commandString);
            Assert.AreEqual(commandContext.Command, command);
        }

        string commandString ="NORTH";
        Command command = new Command("NORTH", "NORTH", (game, CommandContext) => Console.WriteLine("Move North"));
    }
}
