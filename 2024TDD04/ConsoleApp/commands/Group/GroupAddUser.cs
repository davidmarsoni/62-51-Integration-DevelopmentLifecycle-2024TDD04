using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.helpers;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
// ...existing using statements...
namespace ConsoleApp.commands.Group
{
    public class GroupAddUser : ISubCommand
    {
        private readonly GroupService groupService;
        private readonly UserGroupService userGroupService;
        public static string CommandName => "adduser";

        public GroupAddUser(GroupService groupService, UserGroupService userGroupService)
        {
            this.groupService = groupService;
            this.userGroupService = userGroupService;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"Add User to Group\" process...");
            int groupId = InputHelper.PromptForInt("Group Id", "To add a user to a group, input the Group Id. (or type 'exit')");
            if (groupId == -1) return;

            int userId = InputHelper.PromptForInt("User Id", "To add a user to a group, input the User Id. (or type 'exit')");
            if (userId == -1) return;

            UserGroupDTO userGroupDTO = new UserGroupDTO
            {
                GroupId = groupId,
                UserId = userId
            };

            if (userGroupService.AddUserToGroup(userGroupDTO).Result)
                Console.WriteLine(Colors.Colorize("Successfully added the user to the group.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize("An error occurred when adding the user to the group...", Colors.Red));
       
        }

        public string GetDescription() => $"{CommandName} : Add a user to a group.";
    }
}
