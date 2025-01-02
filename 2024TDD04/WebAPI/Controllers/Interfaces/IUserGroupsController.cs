using Microsoft.AspNetCore.Mvc;
using DTO;

namespace WebApi.Controllers.Interfaces
{
    public interface IUserGroupsController
    {
        Task<ActionResult<UserGroupDTO>> GetUserGroup(int id);
        Task<ActionResult<IEnumerable<UserDTO>>> GetUsersInGroup(int groupId);
        Task<ActionResult<IEnumerable<GroupDTO>>> GetGroupsForUser(int userId);
        Task<ActionResult<UserGroupDTO>> PostUserGroup(UserGroupDTO userGroupDTO);
        Task<IActionResult> DeleteUserFromGroup(int userId, int groupId);
    }
}
