using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using MVC.Services;
using static ConsoleApp.utils.ConsoleUtils;
using ConsoleApp.commands.User;

namespace ConsoleApp.commands.Room
{
    public class RoomEdit : ISubCommand
    {
        private readonly RoomService roomService;
        public static string CommandName => "edit";

        public RoomEdit(RoomService roomService)
        {
            this.roomService = roomService;
        }

        public void Execute(string[] arguments)
        {
            Title("Edit a room");
            int roomId = InputUtils.PromptForInt("Room Id", "Enter the Room ID (or type 'exit')");
            if (roomId == -1) return;

            var roomDTO = roomService.GetRoomById(roomId).Result;
            if (roomDTO == null)
            {
                Console.WriteLine(Colors.Colorize("Room not found. Exiting...", Colors.Red));
                return;
            }

            string promptName = "Enter the new " + Colors.Colorize($"Name ({roomDTO.Name})", Colors.Yellow) + " of the room or leave empty to keep the current name. (or type 'exit')";
            var (exit, newName) = InputUtils.PromptForString("Name", 
                promptName, 
                (input) => RoomUtils.ValidateName(roomService, input),
                true);
            if (exit) return;

            if (!string.IsNullOrEmpty(newName)) roomDTO.Name = newName;

            string promptAbbrev = "Enter the new " + Colors.Colorize($"Abreviation ({roomDTO.RoomAbreviation})", Colors.Yellow) + " of the room or leave empty to delete the current abbreviation. (or type 'exit')";
            var (exit2, newAbbrev) = InputUtils.PromptForString("Abreviation", 
                promptAbbrev, 
                (input) => RoomUtils.ValidateAbbreviation(roomService, input),
                true);
            if (exit2) return;

            if (!string.IsNullOrEmpty(newAbbrev)) roomDTO.RoomAbreviation = newAbbrev;

            if (roomService.UpdateRoom(roomDTO).Result)
                Success("Successfully edited the room.");
            else
                Error("An error occurred when editing the room...");
        }

        public string GetDescription() => $"{CommandName} : Edit an existing room.";
    }
}
