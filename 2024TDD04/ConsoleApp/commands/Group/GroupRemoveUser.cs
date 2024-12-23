using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.helpers;
using ConsoleApp.utils;
using MVC.Services;
// ...existing using statements...
namespace ConsoleApp.commands.Group
{
    public class GroupRemoveUser : ISubCommand
    {
        private readonly GroupService groupService;
        private readonly UserGroupService userGroupService;
        public static string CommandName => "removeuser";

        public GroupRemoveUser(GroupService groupService, UserGroupService userGroupService)
        {
            this.groupService = groupService;
            this.userGroupService = userGroupService;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"Remove User from Group\" process...");
            int groupId = InputHelper.PromptForInt("Group Id", "To remove a user from a group, input the Group Id. (or type 'exit')");
            if (groupId == -1) return;

            int userId = InputHelper.PromptForInt("User Id", "To remove a user from a group, input the User Id. (or type 'exit')");
            if (userId == -1) return;

            if (userGroupService.RemoveUserFromGroup(groupId, userId).Result)
                Console.WriteLine(Colors.Colorize("Successfully removed the user from the group.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize("An error occurred when removing the user from the group...", Colors.Red));
        }

        public string GetDescription() => $"{CommandName} : Remove a user from a group.";
    }
}
