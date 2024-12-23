using ConsoleApp.commands.interfaces;
using MVC.Services;
using ConsoleApp.utils;
using ConsoleApp.console;
using ConsoleApp.helpers;

namespace ConsoleApp.commands.Access
{
    public class AccessList : ISubCommand
    {
        private readonly AccessService accessService;
        public static string CommandName => "list";

        public AccessList(AccessService accessService)
        {
            this.accessService = accessService;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"List Accesses\" process...");
            Console.WriteLine("List rooms accessible by a " + Colors.Colorize("User", Colors.Yellow) + " or a " + Colors.Colorize("Group", Colors.Yellow) + "? (user/group)");
            string choice = ConsoleManager.WaitInput(input => input == "user" || input == "group", "Type 'user' or 'group' (or 'exit')");
            if (ConsoleUtils.ExitOnInputExit(choice, "Exiting list accesses process."))
                return;

            if (choice == "user")
            {
                int userId = InputHelper.PromptForInt("User Id", "Enter the User ID (or type 'exit')");
                if (userId == -1) return;

                var room = accessService.GetRoomAccessibleByUser(userId).Result;

                if (room != null)
                    Console.WriteLine($"User {userId} has access to room: {room.Id} - {room.Name}");
                else
                    Console.WriteLine($"No accessible rooms found for user {userId}.");
            }
            else if (choice == "group")
            {
                int groupId = InputHelper.PromptForInt("Group Id", "Enter the Group ID (or type 'exit')");
                if (groupId == -1) return;

                var room = accessService.GetRoomAccessibleByGroup(groupId).Result;

                if (room != null)
                    Console.WriteLine($"Group {groupId} has access to room: {room.Id} - {room.Name}");
                else
                    Console.WriteLine($"No accessible rooms found for group {groupId}.");
            }
        }

        public string GetDescription() => $"{CommandName} : List rooms accessible by a user or a group.";

    }
}