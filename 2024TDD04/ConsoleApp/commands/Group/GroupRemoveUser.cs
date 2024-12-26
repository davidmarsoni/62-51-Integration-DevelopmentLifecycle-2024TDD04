using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
using static ConsoleApp.utils.ConsoleUtils;
namespace ConsoleApp.commands.Group
{
    public class GroupRemoveUser : ISubCommand
    {
        private readonly GroupService groupService;
        private readonly UserGroupService userGroupService;
        private readonly UserService userService;
        public static string CommandName => "removeuser";

        public GroupRemoveUser(GroupService groupService, UserGroupService userGroupService, UserService userService)
        {
            this.groupService = groupService;
            this.userGroupService = userGroupService;
            this.userService = userService;
        }

        public void Execute(string[] arguments)
        {
            Title("Remove User from Group");
            int groupId = InputUtils.PromptForInt("Group Id", "To remove a user from a group, input the Group Id. (or type 'exit')");
            if (groupId == -1) return;

            //verify group exists and is not deleted
            GroupDTO groupDTO = groupService.GetGroupById(groupId).Result;
            if (groupDTO == null || groupDTO.IsDeleted)
            {
                Error("The group with the given ID does not exist.");
                return;
            }

            int userId = InputUtils.PromptForInt("User Id", "To remove a user from a group, input the User Id. (or type 'exit')");
            if (userId == -1) return;

            //verify user exists and is not deleted
            UserDTO userDTO = userService.GetUserById(userId).Result;
            if (userDTO == null || userDTO.IsDeleted)
            {
                Error("The user with the given ID does not exist.");
                return;
            }

            if (userGroupService.RemoveUserFromGroup(groupId, userId).Result)
                Success("Successfully removed the user from the group.");
            else
                Error("An error occurred when removing the user from the group...");
        }

        public string GetDescription() => $"{CommandName} : Remove a user from a group.";
    }
}
