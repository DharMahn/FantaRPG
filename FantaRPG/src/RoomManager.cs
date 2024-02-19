using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantaRPG.src
{
    internal class RoomManager
    {
        private static RoomManager _instance;
        public static RoomManager Instance => _instance ??= new RoomManager();

        // Changed to an instance variable
        private List<(Room Room, int SelectionCount)> rooms;

        // Constructor is private to enforce singleton pattern
        private RoomManager()
        {
            rooms = [];
        }

        public Room GetRandomRoom()
        {
            if (rooms.Count == 0)
            {
                throw new InvalidOperationException("There are no rooms available.");
            }

            double totalWeight = rooms.Sum(x => 1.0 / (1 + x.SelectionCount));
            double value = RNG.GetDouble() * totalWeight;
            double cumulativeWeight = 0;

            foreach (var (Room, SelectionCount) in rooms)
            {
                cumulativeWeight += 1.0 / (1 + SelectionCount);
                if (value < cumulativeWeight)
                {
                    IncrementRoomSelectionCount(Room);
                    return Room;
                }
            }

            var fallbackRoom = rooms.Last().Room;
            IncrementRoomSelectionCount(fallbackRoom);
            return fallbackRoom;
        }

        // Changed to an instance method
        public void AddRoom(Room room)
        {
            rooms.Add((room, 0));
        }

        // New method for adding multiple rooms
        public void AddRooms(IEnumerable<Room> newRooms)
        {
            foreach (var room in newRooms)
            {
                rooms.Add((room, 0));
            }
        }

        private void IncrementRoomSelectionCount(Room room)
        {
            int index = rooms.FindIndex(x => x.Room == room);
            if (index != -1)
            {
                var (selectedRoom, count) = rooms[index];
                rooms[index] = (selectedRoom, count + 1);
            }
        }

        public void ResetRoomSelectionCounts()
        {
            rooms = rooms.Select(x => (x.Room, 0)).ToList();
        }
    }

}
