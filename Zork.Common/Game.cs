using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace Zork
{
    public class Game
    {
        public event EventHandler<bool> IsRunningChanged;

        public World World { get; set; }

        [JsonIgnore]
        public Player Player { get; private set; }

        [JsonIgnore]
        public bool IsRunning
        {
            get => mIsRunning;
            set
            {
                if (mIsRunning != value)
                {
                    mIsRunning = value;
                    IsRunningChanged?.Invoke(this, mIsRunning);
                }
            }
        }

        [JsonIgnore]
        public CommandManager CommandManager { get; }

        public string StartingLocation { get; set; }

        public string WelcomeMessage { get; set; }

        public string EndingMessage { get; set; }

        public IOutputService OutputService { get; private set; }

        public IInputService InputService { get; private set; }

        [OnDeserialized]
        private void OnDeserialize(StreamingContext context)
        {
            Player = new Player(World, StartingLocation);
            Player.PreviousRoom = Player.Location;
        }

        public Game(World world, Player player, IOutputService outputService, IInputService inputService)
        {
            World = world;
            Player = player;
            OutputService = outputService;
            InputService = inputService;
        }

        public Game()
        {
            Command[] commands =
            {
                new Command("LOOK", new string[] { "LOOK","L"},(game, commandContext)=>Look(game)),
                new Command("QUIT", new string[] { "QUIT","Q"},(game, CommandContext)=> game.IsRunning=false),
                new Command("SCORE", new string[] { "SCORE" },(game, CommandContext)=> OutputService.WriteLine($"Move: {Player.Moves}, Score:{Player.Score}")),
                new Command("NORTH", new string[] { "NORTH","N"},(game, CommandContext)=> Move(Directions.NORTH)),
                new Command("SOUTH", new string[] { "SOUTH","S"},(game, CommandContext)=> Move(Directions.SOUTH)),
                new Command("EAST", new string[] { "EAST","E"},(game, CommandContext)=> Move(Directions.EAST)),
                new Command("WEST", new string[] { "WEST","W"},(game, CommandContext)=> Move(Directions.WEST)),
                new Command("UP", new string[] { "UP","U"},(game, CommandContext)=> Move(Directions.UP)),
                new Command("DOWN", new string[] { "DOWN","D"},(game, CommandContext)=> Move(Directions.DOWN)),
                new Command("TAKE", new string[] { "TAKE" }, (game,CommandContext) => Take()),
                new Command("DROP", new string[] { "DROP" }, (game,CommandContext) => Drop()),
                new Command("ITEM", new string[] { "ITEM" },(game,CommandContext) => Item()),
                new Command("EAT", new string[] { "EAT"},(game, CommandContext) => Eat()),
                new Command("ATTACK", new string[] { "ATTACK"}, (game,CommandContext) => Attack()),
            };

            CommandManager = new CommandManager(commands);
        }

        public void Attack()
        {
            if (Player.Location._Enemy.Name == null)
            {
                OutputService.WriteLine($"Nobody is here, who are you trying to attack?");
            }
            else if (Player.Item.Count == 0 || Player.Item == null)
            {
                OutputService.WriteLine($"You don't have anything! Don't try to attack {Player.Location._Enemy.Name} with your bare hands!");
            }
            else
            {
                {
                    isAttacking = true;
                    OutputService.WriteLine($"What do you try to attack with?");
                }
            }
        }

        public void WeaponAttack()
        {
            foreach (Item item in Player.Item)
            {
                if (itemText.Equals(item.Name, StringComparison.OrdinalIgnoreCase) && item.Damage > 0)
                {
                    Player.Location._Enemy.HitPoints -= item.Damage;
                    OutputService.AudioOutput("Attack");
                    OutputService.WriteLine($"You attack {Player.Location._Enemy.Name} by {item.Damage}.");
                    OutputService.EnemyOutput(Player, Player.Location._Enemy.HitPoints, Player.Location._Enemy.MaxHitPoints);
                    if (Player.Location._Enemy.HitPoints <= 0)
                    {
                        OutputService.WriteLine($"{Player.Location._Enemy.DefeatMessage}");
                        Player.AddScore(Player.Location._Enemy.DefeatScore);
                        Player.Location._Enemy = null;
                    }
                    isAttacking = false;
                    return;
                }
                else if (itemText.Equals(item.Name, StringComparison.OrdinalIgnoreCase) && item.Damage == 0)
                {
                    OutputService.WriteLine($"You can't attack with {item.Name}.");
                    isAttacking = false;
                    return;
                }
            }
            OutputService.WriteLine($"I don't think you have {itemText}.");
            isAttacking = false;
        }

        public void Eat()
        {
            if (itemText == null)
            {
                OutputService.WriteLine($"I'm not sure what you're trying to eat.");
            }
            else if (Player.Item.Count == 0)
            {
                OutputService.WriteLine($"You don't have {itemText}");
            }
            else
            {
                foreach (Item item in Player.Item)
                {
                    if (itemText.Equals(item.Name, StringComparison.OrdinalIgnoreCase) && item.Eatable == true)
                    {
                        OutputService.WriteLine($"You eat {item.Name}");
                        OutputService.AudioOutput("Eat");
                        Player.Item.Remove(item);
                        return;
                    }
                    else if (itemText.Equals(item.Name, StringComparison.OrdinalIgnoreCase) && item.Eatable == false)
                    {
                        OutputService.WriteLine($"You can't eat {item.Name}");
                        return;
                    }
                }
                OutputService.WriteLine($"You don't have {itemText}");
            }
        }
        
        public void Move(Directions direction)
        {
            if (Player.Move(direction) == false)
            {
                OutputService.WriteLine($"The way is shut!\n{Player.Location}");
            }
        }

        public void Item()
        {
            if (Player.Item.Count == 0)
            {
                OutputService.WriteLine("What are you looking for? You don't have anything!");
                return;
            }
            OutputService.WriteLine("You got:");
            foreach (Item item in Player.Item)
            {
                OutputService.WriteLine(item.Name);
            }
        }

        public void Take()
        {
            if (itemText != null)
            {
                if (Player.Location.ItemList.Count == 0)
                {
                    OutputService.WriteLine($"There's nothing you can take in {Player.Location}.");
                    return;
                }
                if (Player.Location._Enemy != null && itemText.Equals(Player.Location._Enemy.Name, StringComparison.OrdinalIgnoreCase))
                {
                    OutputService.WriteLine($"Don't try to get {Player.Location._Enemy.Name}!");
                    return;
                }
                foreach (Item item in Player.Location.ItemList)
                {
                    if (itemText.Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        OutputService.WriteLine($"You take the {itemText}");
                        Player.Location.ItemList.Remove(item);
                        Player.Item.Add(item);
                        OutputService.ItemOutput(Player);
                        return;
                    }
                }
                OutputService.WriteLine($"There's no {itemText} in {Player.Location}");
            }
            else
            {
                OutputService.WriteLine("What do you want to take?");
            }
        }

        public void Drop()
        {
            if (Player.Location.ItemList.Count >=5)
            {
                OutputService.WriteLine($"There's too many item in the {Player.Location.Name}, no room for the other one.");
            }
            else
            { 
                if (itemText != null)
                {
                    foreach (Item item in Player.Item)
                    {
                        if (itemText.Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            OutputService.WriteLine($"You drop the {itemText}");
                            Player.Location.ItemList.Add(item);
                            Player.Item.Remove(item);
                            OutputService.ItemOutput(Player);
                            return;
                        }
                    }
                    OutputService.WriteLine($"You don't have {itemText}.");
                }
                else
                {
                    OutputService.WriteLine("What do you want to drop?");
                }
            }
        }

        public void Look(Game game)
        {
            OutputService.WriteLine($"{game.Player.Location}\n{game.Player.Location.Description}");

            if (Player.Location.ItemList != null && Player.Location.ItemList.Count !=0)
            {
                int inventoryCount = 0;
                OutputService.Write($"There are something lying on the ground: ");
                foreach (Item inventory in Player.Location.ItemList)
                {
                    if (Player.Location.ItemList.Count == 1)
                    {
                        OutputService.WriteLine($"{inventory.Name}.");
                    }
                    else if (inventoryCount < Player.Location.ItemList.Count-1)
                    {
                        OutputService.Write($"{inventory.Name}, ");
                    }
                    else
                    {
                        OutputService.WriteLine($"and {inventory.Name}.");
                    }
                    inventoryCount++;
                }
            }

            if (Player.Location._Enemy != null && Player.Location._Enemy.Name != null)
            {
                OutputService.WriteLine($"There is a {Player.Location._Enemy.Name} in the center of {Player.Location.Name}.");
            }
        }

        public static Game Load(string jsonString, IOutputService outputService, IInputService inputService)
        {
            Game game = JsonConvert.DeserializeObject<Game>(jsonString);

            if (outputService == null)
            {
                throw new ArgumentNullException(nameof(outputService));
            }

            if (inputService == null)
            {
                throw new ArgumentException(nameof(inputService));
            }

            game.OutputService = outputService;
            game.InputService = inputService;
            game.InputService.InputReceived += game.InputReceivedHandler;

            game.OutputService.WriteLine(game.WelcomeMessage);
            game.CommandManager.PerformCommand(game, "LOOK");
            game.IsRunning = true;

            game.Player.LocationChanged += game.PlayerLocationChangedHandler;
            game.Player.ScoreChanged += game.PlayerScoreChangedHandler;
            game.IsRunningChanged += game.IsRunningChangedHandler;

            return game;
        }

        public static Game StartFromFile(string gameFileName, IOutputService outputService, IInputService inputService)
        {
            if (!File.Exists(gameFileName))
            {
                throw new FileNotFoundException("Expected file.", gameFileName);
            }

            return Load(File.ReadAllText(gameFileName),outputService,inputService);
        }

        private void PlayerScoreChangedHandler(object sender, int e)
        {
            OutputService.ScoreOutput(Player.mScore);
        }

        private void PlayerLocationChangedHandler(object sender, Room e)
        {
            if (sender is Player player)
            {
                Player.Moves++;
                OutputService.MoveOutput(Player.Moves);
                OutputService.LocationOutput(Player.Location.ToString());
                OutputService.BackgroundOutput(Player);
                OutputService.ItemOutput(Player);
                OutputService.AudioOutput("Move");
                if (Player.Location._Enemy != null)
                { 
                    OutputService.EnemyOutput(Player, Player.Location._Enemy.HitPoints, Player.Location._Enemy.MaxHitPoints);
                }
            }
        }

        private void InputReceivedHandler(object sender, string inputString)
        {
            itemText= null;
            if (isAttacking == true)
            {
                itemText = inputString;
                WeaponAttack();
            }
            else
            { 
                if (inputString.Contains(" "))
                {
                    string[] seperateText = inputString.Split(' ');
                    inputString = seperateText[0];
                    itemText = seperateText[1];
                }
                if (CommandManager.PerformCommand(this, inputString))
                {
                    if (Player.PreviousRoom != Player.Location)
                    {
                       CommandManager.PerformCommand(this, "LOOK");
                        Player.PreviousRoom = Player.Location;
                    }
                }
                else
                {
                    OutputService.WriteLine("That's not a verb I recognized.");
                }            
            }
        }

        private void IsRunningChangedHandler(object sender, bool e)
        {
            OutputService.QuitGame();
        }

        public bool mIsRunning = true;
        string itemText;
        bool isAttacking = false;
    }
}
