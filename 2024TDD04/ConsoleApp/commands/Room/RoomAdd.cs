using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.utils;
using DTO;
using MVC.Services;

namespace ConsoleApp.commands.Room
{
    public class RoomAdd : ISubCommand
    {
        private readonly RoomService roomService;
        public static string CommandName => "add";

        public RoomAdd(RoomService roomService)
        {
            this.roomService = roomService;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"Add Room\" process...");
            // ...logic from AddRoom...
            string nameInput = ConsoleManager.WaitInput(ValidateRoomNameIsUnique,
                "Enter the room name (or type 'exit')").Trim();
            if (ConsoleUtils.ExitOnInputExit(nameInput, "Exiting room creation."))
                return;
            Console.WriteLine("Enter an optional Abreviation for the room.");
            string abreviationInput = ConsoleManager.WaitInput(ValidateRoomAbreviationIsUnique,
                "(Press enter to skip)").Trim();
            if (ConsoleUtils.ExitOnInputExit(abreviationInput, "Exiting room creation."))
                return;

            RoomDTO roomDTO = new()
            {
                Name = nameInput,
                RoomAbreviation = string.IsNullOrEmpty(abreviationInput) ? null : abreviationInput
            };
            if (roomService.CreateRoom(roomDTO).Result != null)
                Console.WriteLine(Colors.Colorize("Successfully added the room.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize("An error occurred when adding the room to the DB...", Colors.Red));
        }

        private bool ValidateRoomNameIsUnique(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            if (input.ToLower() == "exit") return true;
            if (roomService.RoomNameExists(input).Result)
            {
                Console.WriteLine(Colors.Colorize("Room name already exists. Choose a different name.", Colors.Red));
                return false;
            }
            return true;
        }

        private bool ValidateRoomAbreviationIsUnique(string input)
        {
            if (string.IsNullOrEmpty(input) || input.ToLower() == "exit") return true;
            if (roomService.RoomAbreviationExists(input).Result)
            {
                Console.WriteLine(Colors.Colorize("Room abbreviation already exists.", Colors.Red));
                return false;
            }
            return true;
        }

        public string GetDescription() => $"{CommandName} : Add a new room.";
    }
}
