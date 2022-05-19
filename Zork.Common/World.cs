using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Zork
{
    public class World
    {
        public Room[] Rooms { get; set; }

        [JsonIgnore]
        public Dictionary<string, Room> RoomsByName { get; private set; }

        [OnDeserialized]
        private void OnDeserialize(StreamingContext context)
        {
            RoomsByName = Rooms.ToDictionary(room => room.Name, room => room);

            foreach (Room room in Rooms)
            {
                room.UpdateNeighbors(this);
            }
        }
    }
}
