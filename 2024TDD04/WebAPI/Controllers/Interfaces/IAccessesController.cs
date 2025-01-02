using Microsoft.AspNetCore.Mvc;
using DTO;

namespace WebApi.Controllers.Interfaces
{
    public interface IAccessesController
    {
        Task<ActionResult<bool>> HasAccessGroupAsync(int roomId, int groupId);
        Task<ActionResult<bool>> HasAccessUserAsync(int roomId, int userId);
        Task<ActionResult<IEnumerable<RoomDTO>>> GetAccessesByGroupId(int groupId);
        Task<ActionResult<IEnumerable<RoomDTO>>> GetAccessesByUserId(int userId);
        Task<ActionResult<Boolean>> GrantAccessAsync(AccessDTO accessDTO);
        Task<ActionResult<Boolean>> RevokeAccessAsync(int roomId, int groupId);
    }
}
