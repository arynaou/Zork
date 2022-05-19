using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Zork
{
    public class Player
    {
        public event EventHandler<int> ScoreChanged;
        public event EventHandler<Room> LocationChanged;

        public World World { get;}
        
        public int Moves { get; set; }
        
        public Room PreviousRoom { get; set; }

        public List<Item> Item { get; set; }

        [JsonIgnore]
        public Room Location 
        { 
            get => mLocation;
            private set
            {
                if (mLocation != value)
                {
                    mLocation = value;
                    LocationChanged?.Invoke(this, mLocation);
                }
            }
        }

        public int Score
        {
            get => mScore;
            set
            {
                if (mScore != value)
                {
                    mScore = value;
                    ScoreChanged?.Invoke(this, mScore);
                }
            }
        }

        public Player(World world, string startingLocation)
        {
            Moves = 0;
            World = world;
            Location = World.RoomsByName[startingLocation];
            Item = new List<Item>();
        }

        public bool Move(Directions direction)
        {
            bool isValidMove = Location.Neighbors.TryGetValue(direction, out Room room);

            if (isValidMove)
            {
                Location = room;
            }
            return isValidMove;
        }

        public void AddScore(int score)
        {
            Score += score;
        }

        public Room mLocation;
        public int mScore;
    }
}
