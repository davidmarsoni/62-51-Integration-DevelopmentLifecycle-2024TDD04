using ConsoleApp.commands.interfaces;
using MVC.Services;
using ConsoleApp.utils;
using DTO;
using ConsoleApp.helpers;

namespace ConsoleApp.commands.Access
{
    public class AccessRevoke : ISubCommand
    {
        private readonly AccessService accessService;
        public static string CommandName => "revoke";

        public AccessRevoke(AccessService accessService)
        {
            this.accessService = accessService;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"Revoke Access\" process...");
            int roomId = InputHelper.PromptForInt("Room Id", "Enter the Room ID (or type 'exit')");
            if (roomId == -1) return;

            int groupId = InputHelper.PromptForInt("Group Id", "Enter the Group ID (or type 'exit')");
            if (groupId == -1) return;

            AccessDTO accessDTO = new AccessDTO
            {
                RoomId = roomId,
                GroupId = groupId
            };

            if (accessService.RevokeAccessAsync(accessDTO).Result)
                Console.WriteLine(Colors.Colorize("Successfully revoked access.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize("An error occurred while revoking access.", Colors.Red));
        }

        public string GetDescription() => $"{CommandName} : Revoke access from a group for a room.";
    }
}