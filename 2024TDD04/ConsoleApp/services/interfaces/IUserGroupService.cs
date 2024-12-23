using DTO;

namespace MVC.Services.Interfaces
{
    public interface IUserGroupService
    {
        Task<bool> AddUserToGroup(UserGroupDTO userGroupDTO);
        Task<bool> RemoveUserFromGroup(int groupId, int userId);
        Task<IEnumerable<UserDTO>?> GetUsersInGroup(int groupId);
    }
}
