using ConsoleApp.commands.interfaces;
using DTO;
using MVC.Services;
using ConsoleApp.utils;
using static ConsoleApp.utils.ConsoleUtils; // new using

namespace ConsoleApp.commands.Room
{
    public class RoomList : ISubCommand
    {
        private readonly RoomService roomService;
        public static string CommandName => "list";

        public RoomList(RoomService roomService)
        {
            this.roomService = roomService;
        }

        public void Execute(string[] arguments)
        {
            Title("List existing rooms");
            IEnumerable<RoomDTO>? roomDTOs = roomService.GetAllRooms().Result;
            EntityCommandUtils.ListEntities(roomDTOs, "Room", room =>
            {
                string abreviation = string.IsNullOrEmpty(room.RoomAbreviation) ? "" : $" ({room.RoomAbreviation})";
                return $"{room.Id} - {room.Name}{abreviation}";
            });
        }

        public string GetDescription() => $"{CommandName} : List all rooms.";
    }
}
