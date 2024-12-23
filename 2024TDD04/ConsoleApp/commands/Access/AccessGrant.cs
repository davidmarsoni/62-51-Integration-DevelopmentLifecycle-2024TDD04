using ConsoleApp.commands.interfaces;
using MVC.Services;
using ConsoleApp.utils;
using DTO;
using ConsoleApp.helpers;

namespace ConsoleApp.commands.Access
{
    public class AccessGrant : ISubCommand
    {
        private readonly AccessService accessService;
        public static string CommandName => "grant";

        public AccessGrant(AccessService accessService)
        {
            this.accessService = accessService;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"Grant Access\" process...");
            int roomId = InputHelper.PromptForInt("Room Id", "Enter the Room ID (or type 'exit')");
            if (roomId == -1) return;

            int groupId = InputHelper.PromptForInt("Group Id", "Enter the Group ID (or type 'exit')");
            if (groupId == -1) return;

            AccessDTO accessDTO = new AccessDTO
            {
                RoomId = roomId,
                GroupId = groupId
            };

            if (accessService.GrantAccessAsync(accessDTO).Result)
                Console.WriteLine(Colors.Colorize("Successfully granted access.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize("An error occurred while granting access.", Colors.Red));
        }

        public string GetDescription() => $"{CommandName} : Grant access to a group for a room.";
    }
}