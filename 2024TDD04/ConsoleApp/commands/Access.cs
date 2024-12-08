using ConsoleApp.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using MVC.Services;
using MVC.Services.Interfaces;
using ConsoleApp.utils;
using ConsoleApp.console;

namespace ConsoleApp.commands
{
    public class Access : ICommand
    {
        private AccessService accessService;

        public Access(HttpClient httpClient, string baseURL, bool debug)
        {
            accessService = new AccessService(httpClient, baseURL, debug);
        }

        public void Execute(string[] arguments)
        {
            if (arguments.Length == 0)
            {
                Console.WriteLine("Access : " + Colors.Colorize("No arguments provided", Colors.Red));
                return;
            }
            switch (arguments[0])
            {
                case "grant":
                    GrantAccess();
                    break;
                case "revoke":
                    RevokeAccess();
                    break;
                case "list":
                    ListAccesses();
                    break;
                case "test":
                    TestAccess();
                    break;
                default:
                    Console.WriteLine("Access : " + Colors.Colorize("Command not found", Colors.Red));
                    break;
            }
        }

        public string GetDescription()
        {
            return "access - Manage access to rooms.";
        }

        public string GetSubCommands()
        {
            return "grant, revoke, list, test";
        }

        private void GrantAccess()
        {
            Console.WriteLine("Beginning the \"Grant Access\" process...");
            // Prompt for RoomId
            Console.WriteLine("Enter the " + Colors.Colorize("Room Id", Colors.Yellow) + " to grant access to.");
            string roomIdInput = ConsoleManager.WaitInput(EntityCommandUtils.ValidationIdIsInt, "Enter the Room ID (or type 'exit')");
            if (ConsoleUtils.ExitOnInputExit(roomIdInput, "Exiting grant access process."))
                return;

            // Prompt for GroupId
            Console.WriteLine("Enter the " + Colors.Colorize("Group Id", Colors.Yellow) + " to grant access to.");
            string groupIdInput = ConsoleManager.WaitInput(EntityCommandUtils.ValidationIdIsInt, "Enter the Group ID (or type 'exit')");
            if (ConsoleUtils.ExitOnInputExit(groupIdInput, "Exiting grant access process."))
                return;

            int roomId = int.Parse(roomIdInput);
            int groupId = int.Parse(groupIdInput);

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

        private void RevokeAccess()
        {
            Console.WriteLine("Beginning the \"Revoke Access\" process...");
            // Prompt for RoomId
            Console.WriteLine("Enter the " + Colors.Colorize("Room Id", Colors.Yellow) + " to revoke access from.");
            string roomIdInput = ConsoleManager.WaitInput(EntityCommandUtils.ValidationIdIsInt, "Enter the Room ID (or type 'exit')");
            if (ConsoleUtils.ExitOnInputExit(roomIdInput, "Exiting revoke access process."))
                return;

            // Prompt for GroupId
            Console.WriteLine("Enter the " + Colors.Colorize("Group Id", Colors.Yellow) + " to revoke access from.");
            string groupIdInput = ConsoleManager.WaitInput(EntityCommandUtils.ValidationIdIsInt, "Enter the Group ID (or type 'exit')");
            if (ConsoleUtils.ExitOnInputExit(groupIdInput, "Exiting revoke access process."))
                return;

            int roomId = int.Parse(roomIdInput);
            int groupId = int.Parse(groupIdInput);

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

        private void ListAccesses()
        {
            Console.WriteLine("Beginning the \"List Accesses\" process...");
            Console.WriteLine("List rooms accessible by a " + Colors.Colorize("User", Colors.Yellow) + " or a " + Colors.Colorize("Group", Colors.Yellow) + "? (user/group)");
            string choice = ConsoleManager.WaitInput(input => input == "user" || input == "group", "Type 'user' or 'group' (or 'exit')");
            if (ConsoleUtils.ExitOnInputExit(choice, "Exiting list accesses process."))
                return;

            if (choice == "user")
            {
                // Prompt for UserId
                Console.WriteLine("Enter the " + Colors.Colorize("User Id", Colors.Yellow) + " to list accessible rooms for.");
                string userIdInput = ConsoleManager.WaitInput(EntityCommandUtils.ValidationIdIsInt, "Enter the User ID (or type 'exit')");
                if (ConsoleUtils.ExitOnInputExit(userIdInput, "Exiting list accesses process."))
                    return;

                int userId = int.Parse(userIdInput);
                var room = accessService.GetRoomAccessibleByUser(userId).Result;

                if (room != null)
                    Console.WriteLine($"User {userId} has access to room: {room.Id} - {room.Name}");
                else
                    Console.WriteLine($"No accessible rooms found for user {userId}.");
            }
            else if (choice == "group")
            {
                // Prompt for GroupId
                Console.WriteLine("Enter the " + Colors.Colorize("Group Id", Colors.Yellow) + " to list accessible rooms for.");
                string groupIdInput = ConsoleManager.WaitInput(EntityCommandUtils.ValidationIdIsInt, "Enter the Group ID (or type 'exit')");
                if (ConsoleUtils.ExitOnInputExit(groupIdInput, "Exiting list accesses process."))
                    return;

                int groupId = int.Parse(groupIdInput);
                var room = accessService.GetRoomAccessibleByGroup(groupId).Result;

                if (room != null)
                    Console.WriteLine($"Group {groupId} has access to room: {room.Id} - {room.Name}");
                else
                    Console.WriteLine($"No accessible rooms found for group {groupId}.");
            }
        }

        private void TestAccess()
        {
            // Placeholder for test access functionality
            Console.WriteLine("Test Access functionality is not implemented yet.");
        }
    }
}
