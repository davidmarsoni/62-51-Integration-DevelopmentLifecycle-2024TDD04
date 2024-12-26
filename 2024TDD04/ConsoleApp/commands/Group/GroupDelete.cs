using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using MVC.Services;
using static ConsoleApp.utils.ConsoleUtils;

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
            Title("Delete a group");
            int groupId = InputUtils.PromptForInt("Group Id", "Enter the Group ID (or type 'exit')");
            if (groupId == -1) return;

            var groupDTO = groupService.GetGroupById(groupId).Result;
            if (groupDTO == null || groupDTO.IsDeleted)
            {
                Error("Group not found. Exiting...");
                return;
            }
            Warning("Deleting the group...");
            EntityCommandUtils.ConfirmAndDeleteEntity(groupId, groupService.DeleteGroup, "Group");
        }

        public string GetDescription() => $"{CommandName} : Delete a group.";
        
    }
}
