using Microsoft.AspNetCore.Mvc;
using DTO;

namespace WebApi.Controllers.Interfaces
{
    public interface IUsersController
    {
        Task<ActionResult<List<UserDTO>>> GetUsers();
        Task<ActionResult<Boolean>> GetUsernameExist(string username);
        Task<ActionResult<List<UserDTO>>> GetUsersActive();
        Task<ActionResult<List<UserDTO>>> GetUsersByGroupId(int groupId);
        Task<ActionResult<UserDTO>> GetUser(int id);
        Task<IActionResult> PutUser(int id, UserDTO userDTO);
        Task<ActionResult<UserDTO>> PostUser(UserDTO userDTO);
        Task<IActionResult> DeleteUser(int id);
    }
}
