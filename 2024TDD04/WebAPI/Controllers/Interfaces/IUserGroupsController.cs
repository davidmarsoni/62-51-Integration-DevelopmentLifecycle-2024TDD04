using Microsoft.AspNetCore.Mvc;
using DTO;

namespace WebApi.Controllers.Interfaces
{
    public interface IUserGroupsController
    {
        Task<ActionResult<IEnumerable<UserDTO>>> GetUsersInGroup(int groupId);
        Task<ActionResult<UserGroupDTO>> PostUserGroup(UserGroupDTO userGroupDTO);
        Task<IActionResult> DeleteUserFromGroup(int userId, int groupId);
    }
}
