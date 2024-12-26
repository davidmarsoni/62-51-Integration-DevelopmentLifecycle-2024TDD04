using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
using static ConsoleApp.utils.ConsoleUtils;

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
            Title("List Users in Group");
            int groupId = InputUtils.PromptForInt("Group Id", "To list users in a group, input the Group Id. (or type 'exit')");
            if (groupId == -1) return;

            //verify group exists and is not deleted
            GroupDTO groupDTO = groupService.GetGroupById(groupId).Result;
            if (groupDTO == null || groupDTO.IsDeleted)
            {
                Error("The group with the given ID does not exist.");
                return;
            }

            IEnumerable<UserDTO>? users = userGroupService.GetUsersInGroup(groupId).Result;
            EntityCommandUtils.ListEntities(users, "User", user => $"{user.Id} - {user.Username}");
       
        }

        public string GetDescription() => $"{CommandName} : List users in a group.";
    }
}
