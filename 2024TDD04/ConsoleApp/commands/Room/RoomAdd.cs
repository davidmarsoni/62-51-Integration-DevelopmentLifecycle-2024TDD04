using ConsoleApp.commands.interfaces;
using ConsoleApp.commands.User;
using ConsoleApp.console;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
using static ConsoleApp.utils.ConsoleUtils;

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
            Title("Add a new room");
            var (exit, nameInput) = InputUtils.PromptForString("Name",
                "Enter the " + Colors.Colorize("Name", Colors.Yellow) + " of the room. (or type 'exit')",
                (input) => RoomUtils.ValidateName(roomService, input));
            if (exit) return;
            var (exit2, abbrevInput) = InputUtils.PromptForString("Abreviation",
                "Enter an optional " + Colors.Colorize("Abbreviation", Colors.Yellow) + " for the room. (or type 'exit')",
                (input) => RoomUtils.ValidateAbbreviation(roomService, input),
                true);
            if (exit2) return;

            RoomDTO roomDTO = new() 
            {
                Name = nameInput,
                RoomAbreviation = string.IsNullOrEmpty(abbrevInput) ? null : abbrevInput
            };
            if (roomService.CreateRoom(roomDTO).Result != null)
                Success("Successfully added the room.");
            else
                Error("An error occurred when adding the room to the DB...");
        }

        public string GetDescription() => $"{CommandName} : Add a new room.";
    }
}
