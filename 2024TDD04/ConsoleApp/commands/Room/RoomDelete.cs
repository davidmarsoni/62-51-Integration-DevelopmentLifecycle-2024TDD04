using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.utils;
using MVC.Services;

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
            Console.WriteLine("Beginning the \"Delete Room\" process...");
            // ...logic from DeleteRoom...
            string roomIdInput = ConsoleManager.WaitInput(
                ConsoleUtils.IsIntValidation,
                "To delete a room, input the ID. (or type 'exit')").ToLower();
            if (ConsoleUtils.ExitOnInputExit(roomIdInput, "Exiting room deletion."))
                return;
            if (!int.TryParse(roomIdInput, out int roomId))
            {
                Console.WriteLine(Colors.Colorize("Error parsing RoomId. Exiting...", Colors.Red));
                return;
            }
            EntityCommandUtils.ConfirmAndDeleteEntity(roomId, roomService.DeleteRoom, "Room");
        }

        public string GetDescription() => $"{CommandName} : Delete a room.";
    }
}
