namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            const string defaultRoomsFilename = "Game.json";
            string roomsFilename = (args.Length > 0 ? args[(int)CommandLineArguments.RoomsFilename] : defaultRoomsFilename);

            var outputService = new ConsoleOutputService();
            var inputService = new ConsoleInputService();

            Game game = Game.StartFromFile(roomsFilename, outputService, inputService);

            while (game.IsRunning)
            {
                game.OutputService.Write("\n> ");
                inputService.ProcessInput();
            }
            game.OutputService.Write(game.EndingMessage);
        }

        private enum CommandLineArguments
        {
            RoomsFilename = 0
        }
    }
}
