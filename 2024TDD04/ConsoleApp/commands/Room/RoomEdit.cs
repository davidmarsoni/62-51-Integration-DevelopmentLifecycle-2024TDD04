using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
using ConsoleApp.helpers;

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
            Console.WriteLine("Beginning the \"Edit Room\" process...");
            int roomId = InputHelper.PromptForInt("Room ID", "Input the room ID (or type 'exit')");
            if (roomId == -1) return;

            RoomDTO? roomDTO = roomService.GetRoomById(roomId).Result;
            if (roomDTO == null)
            {
                Console.WriteLine(Colors.Colorize("Room not found. Exiting...", Colors.Red));
                return;
            }
            Console.WriteLine("Enter a new Name for the room. (Press Enter to skip)");
            string nameInput = InputHelper.PromptForString("Name", "(Press Enter to skip)");
            if (ConsoleUtils.ExitOnInputExit(nameInput, "Exiting room editing."))
                return;

            Console.WriteLine("Enter a new Abreviation for the room. (Press Enter to skip)");
            string abreviationInput = InputHelper.PromptForString("Abreviation", "(Press Enter to skip)");
            if (ConsoleUtils.ExitOnInputExit(abreviationInput, "Exiting room editing."))
                return;

            if (string.IsNullOrEmpty(nameInput) && string.IsNullOrEmpty(abreviationInput))
            {
                Console.WriteLine("No changes made. Exiting room editing.");
                return;
            }
            if (!string.IsNullOrEmpty(nameInput))
                roomDTO.Name = nameInput;
            if (!string.IsNullOrEmpty(abreviationInput))
                roomDTO.RoomAbreviation = abreviationInput;

            if (roomService.UpdateRoom(roomDTO).Result)
                Console.WriteLine(Colors.Colorize("Successfully edited the room.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize("An error occurred when editing the room in the DB...", Colors.Red));
        }

        public string GetDescription() => $"{CommandName} : Edit an existing room.";
    }
}
