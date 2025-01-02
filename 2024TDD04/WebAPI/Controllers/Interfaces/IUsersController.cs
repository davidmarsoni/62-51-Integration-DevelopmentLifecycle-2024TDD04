using Microsoft.AspNetCore.Mvc;
using DTO;

namespace WebApi.Controllers.Interfaces
{
    public interface IUsersController
    {
        Task<ActionResult<IEnumerable<UserDTO>>> GetUsers();
        Task<ActionResult<IEnumerable<UserDTO>>> GetUsersActive();
        Task<ActionResult<UserDTO>> GetUser(int id);
        Task<IActionResult> PutUser(int id, UserDTO userDTO);
        Task<ActionResult<UserDTO>> PostUser(UserDTO userDTO);
        Task<IActionResult> DeleteUser(int id);
        Task<ActionResult<bool>> UsernameExist(string username);
    }
}
