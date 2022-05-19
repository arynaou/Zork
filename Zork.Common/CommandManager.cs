using System;
using System.Collections.Generic;
using System.Linq;

namespace Zork
{
    public class CommandManager
    {
        public CommandManager() => _commands = new HashSet<Command>();

        public CommandManager(IEnumerable<Command> commands) => _commands = new HashSet<Command>(commands);

        public bool PerformCommand(Game game, string commandString)
        {
            bool result;
            CommandContext commandContext = Parse(commandString);
            if (commandContext.Command != null)
            {
                commandContext.Command.Action(game, commandContext);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
 
        public CommandContext Parse(string commandString)
        {
            var commandQuery = from command in _commands
                               where command.Verbs.Contains(commandString, StringComparer.OrdinalIgnoreCase)
                               select new CommandContext(commandString, command);

            return commandQuery.FirstOrDefault();
        }

        private HashSet<Command> _commands;
    }
}
