using ConsoleApp.commands.interfaces;
using MVC.Services;
using ConsoleApp.utils;
using ConsoleApp.console;
using static ConsoleApp.utils.ConsoleUtils;
using DTO;

namespace ConsoleApp.commands.Access
{
    public class AccessList : ISubCommand
    {
        private readonly AccessService accessService;
        private readonly GroupService groupService;
        private readonly UserService userService;
        public static string CommandName => "list";

        public AccessList(AccessService accessService, GroupService groupService, UserService userService)
        {
            this.accessService = accessService;
            this.groupService = groupService;
            this.userService = userService;
        }

        public void Execute(string[] arguments)
        {
            Title("List Accesses");
            Console.WriteLine("List rooms accessible by a " + Colors.Colorize("User", Colors.Yellow) + " or a " + Colors.Colorize("Group", Colors.Yellow) + "? (user/group)");
            var (exit, choice) = InputUtils.PromptForString("Choice", "Type 'user' or 'group' (or 'exit')", GroupOrUserVerify);
            
            if (exit) return;

            if (choice == "user")
            {
                int userId = InputUtils.PromptForInt("User Id", "Enter the User ID (or type 'exit')");
                if (userId == -1) return;

                // verify user exists and is not deleted
                UserDTO userDTO = userService.GetUserById(userId).Result;
                if (userDTO == null || userDTO.IsDeleted)
                {
                    Error("The user with the given ID does not exist.");
                    return;
                }

                IEnumerable<RoomDTO> roomDTOs = accessService.GetAccessesByUserId(userId).Result;

                if (roomDTOs != null)
                {
                    Console.WriteLine($"User {userId} has access to the following rooms:");
                    EntityCommandUtils.ListEntities(roomDTOs, "Room", room => $"{room.Id} - {room.Name}");
                }
                else
                    Console.WriteLine($"No accessible rooms found for user {userId}.");
               
            }
            else if (choice == "group")
            {
                int groupId = InputUtils.PromptForInt("Group Id", "Enter the Group ID (or type 'exit')");
                if (groupId == -1) return;

                // verify group exists and is not deleted
                GroupDTO groupDTO = groupService.GetGroupById(groupId).Result;
                if (groupDTO == null || groupDTO.IsDeleted)
                {
                    Error("The group with the given ID does not exist.");
                    return;
                }

                IEnumerable<RoomDTO> roomDTOs = accessService.GetAccessesByGroupId(groupId).Result;

                if (roomDTOs != null)
                {
                    Console.WriteLine($"Group {groupId} has access to the following rooms:");
                    EntityCommandUtils.ListEntities(roomDTOs, "Room", room => $"{room.Id} - {room.Name}");
                }
                else
                    Console.WriteLine($"No accessible rooms found for group {groupId}.");
            }
        }

        private (bool, string) GroupOrUserVerify(string input)
        {
            if(input != "user" && input != "group")
                return (false, "Type 'user' or 'group' (or 'exit')");
            return (true,null);
        }

        public string GetDescription() => $"{CommandName} : List rooms accessible by a user or a group.";

    }
}