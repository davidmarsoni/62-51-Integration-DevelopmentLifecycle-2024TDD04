using Microsoft.AspNetCore.Mvc;
using DTO;

namespace WebApi.Controllers.Interfaces
{
    public interface IUsersController
    {
        Task<ActionResult<IEnumerable<UserDTO>>> GetUsers();
        Task<ActionResult<bool>> GetUsernameExist(string username);
        Task<ActionResult<IEnumerable<UserDTO>>> GetUsersActive();
        Task<ActionResult<IEnumerable<UserDTO>>> GetUsersByGroupId(int groupId);
        Task<ActionResult<UserDTO>> GetUser(int id);
        Task<IActionResult> PutUser(int id, UserDTO userDTO);
        Task<ActionResult<UserDTO>> PostUser(UserDTO userDTO);
        Task<IActionResult> DeleteUser(int id);
    }
}
