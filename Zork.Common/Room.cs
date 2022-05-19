using Newtonsoft.Json;
using System.Collections.Generic;

namespace Zork
{
    public class Room
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Item> ItemList { get; set; }
        public Enemy _Enemy { get; set; }
        public int RoomNum { get; set; }

        [JsonIgnore]
        public Dictionary<Directions, Room> Neighbors { get; private set; } = new Dictionary<Directions, Room>();

        [JsonProperty(PropertyName = "Neighbors")]
        public Dictionary<Directions, string> NeighborNames { get; set; }

        public Room(string name, string description = null, List<Item> itemList = null, Enemy enemy= null, int roomNum=0)
        {
            Name = name;
            Description = description;
            ItemList = itemList;
            _Enemy = enemy;
            RoomNum = roomNum;
        }

        public void UpdateNeighbors(World world)
        {
            Neighbors = new Dictionary<Directions, Room>();
            foreach (var pair in NeighborNames)
            {
                (Directions direction, string name) = (pair.Key, pair.Value);
                Neighbors.Add(direction, world.RoomsByName[name]);
            }
        }

        public override string ToString() => Name;
    }
}
