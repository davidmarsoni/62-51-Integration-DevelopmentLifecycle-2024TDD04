using Microsoft.AspNetCore.Mvc;
using DTO;

namespace WebApi.Controllers.Interfaces
{
    public interface IAccessesController
    {
        Task<ActionResult<bool>> HasAccessAsync(int roomId, int groupId);
        Task<ActionResult<bool>> HasAccessUserAsync(int roomId, int userId);
        Task<ActionResult<Boolean>> GrantAccessAsync(AccessDTO accessDTO);
        Task<ActionResult<Boolean>> RevokeAccessAsync(AccessDTO accessDTO);
    }
}
