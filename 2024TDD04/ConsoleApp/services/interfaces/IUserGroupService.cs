using DTO;

namespace ConsoleApp.services.@interface
{
    public interface IUserGroupService
    {
        Task<bool> AddUserToGroup(UserGroupDTO userGroupDTO);
        Task<bool> RemoveUserFromGroup(int groupId, int userId);
        Task<IEnumerable<UserDTO>?> GetUsersInGroup(int groupId);
        Task<IEnumerable<GroupDTO>?> GetGroupsForUser(int userId);
    }
}
