using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.helpers;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
// ...existing using statements...
namespace ConsoleApp.commands.Group
{
    public class GroupListUsers : ISubCommand
    {
        private readonly GroupService groupService;
        private readonly UserGroupService userGroupService;
        public static string CommandName => "listusers";

        public GroupListUsers(GroupService groupService, UserGroupService userGroupService)
        {
            this.groupService = groupService;
            this.userGroupService = userGroupService;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"List Users in Group\" process...");
            int groupId = InputHelper.PromptForInt("Group Id", "To list users in a group, input the Group Id. (or type 'exit')");
            if (groupId == -1) return;

            IEnumerable<UserDTO>? users = userGroupService.GetUsersInGroup(groupId).Result;
            EntityCommandUtils.ListEntities(users, "User", user => $"{user.Id} - {user.Username}");
       
        }

        public string GetDescription() => $"{CommandName} : List users in a group.";
    }
}
