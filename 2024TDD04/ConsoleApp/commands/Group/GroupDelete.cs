using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.helpers;
using ConsoleApp.utils;
using MVC.Services;
// ...existing using statements...
namespace ConsoleApp.commands.Group
{
    public class GroupDelete : ISubCommand
    {
        private readonly GroupService groupService;
        public static string CommandName => "delete";

        public GroupDelete(GroupService groupService)
        {
            this.groupService = groupService;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"Delete Group\" process...");
            int groupId = InputHelper.PromptForInt("Group Id", "To delete a group, input the ID. (or type 'exit')");
            if (groupId == -1) return;

            EntityCommandUtils.ConfirmAndDeleteEntity(groupId, groupService.DeleteGroup, "Group");
        }

        public string GetDescription() => $"{CommandName} : Delete a group.";
        
    }
}
