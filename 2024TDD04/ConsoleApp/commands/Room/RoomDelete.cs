using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.utils;
using MVC.Services;
using static ConsoleApp.utils.ConsoleUtils;

namespace ConsoleApp.commands.Room
{
    public class RoomDelete : ISubCommand
    {
        private readonly RoomService roomService;
        public static string CommandName => "delete";

        public RoomDelete(RoomService roomService)
        {
            this.roomService = roomService;
        }

        public void Execute(string[] arguments)
        {
            Title("Delete a room");
            int roomId = InputUtils.PromptForInt("Room Id", "Enter the Room ID (or type 'exit')");
            if (roomId == -1) return;

            var roomDTO = roomService.GetRoomById(roomId).Result;
            if (roomDTO == null)
            {
                Error("Room not found. Exiting...");
                return;
            }
            Warning("Deleting the room...");
            EntityCommandUtils.ConfirmAndDeleteEntity(roomId, roomService.DeleteRoom, "Room");
        }

        public string GetDescription() => $"{CommandName} : Delete a room.";
    }
}
